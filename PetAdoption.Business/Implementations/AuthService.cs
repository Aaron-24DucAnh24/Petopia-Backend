using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Models;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Utils;
using PetAdoption.Data.Entities;
using Microsoft.Extensions.Logging;
using Google.Apis.Auth;
using System.Configuration;
using System.Data;
using System.Security.Authentication;

namespace PetAdoption.Business.Implementations
{
  public class AuthService : BaseService, IAuthService
  {
    public AuthService(
      IServiceProvider provider,
      ILogger<AuthService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
      if (await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == request.Email) != null)
      {
        throw new DuplicateNameException();
      }

      var user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Password = request.Password,
      });

      var userConnection = await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
      {
        Id = user.Id,
        AccessToken = TokenUtil.GenerateAccessToken(user, Configuration),
        AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS),
      });

      await UnitOfWork.SaveChangesAsync();

      return new AuthenticationResponse()
      {
        AccessToken = userConnection.AccessToken,
        AccessTokenExpirationDate = userConnection.AccessTokenExpirationDate
      };
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
      var user = await UnitOfWork.Users
        .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password)
        ?? throw new InvalidCredentialException();

      var userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .Where(u => u.Id == user.Id)
        .FirstOrDefaultAsync()
        ?? throw new InvalidCredentialException();

      var accessToken = TokenUtil.GenerateAccessToken(user, Configuration);
      userConnection.AccessToken = accessToken;
      userConnection.AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS);
      userConnection.IsDeleted = false;
      await UnitOfWork.SaveChangesAsync();

      return new AuthenticationResponse()
      {
        AccessToken = accessToken,
        AccessTokenExpirationDate = userConnection.AccessTokenExpirationDate
      };
    }

    public async Task<AuthenticationResponse> GGLoginAsync(GGLoginRequest request)
    {
      var payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId, new GoogleJsonWebSignature.ValidationSettings());
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

      if (user != null)
      {
        return await LoginAsync(new LoginRequest()
        {
          Email = user.Email,
          Password = user.Password
        });
      }

      return await RegisterAsync(new RegisterRequest()
      {
        FirstName = payload.GivenName,
        LastName = payload.FamilyName,
        Password = string.Empty,
        ConfirmPassword = string.Empty
      });
    }

    public async Task<bool> LogoutAsync()
    {
      var userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstOrDefaultAsync(u => u.Id == UserContext.Id)
        ?? throw new ConfigurationErrorsException();

      userConnection.IsDeleted = true;
      await UnitOfWork.SaveChangesAsync();

      return true;
    }

    public bool ValidateAccessToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      SecurityToken? securityToken;
      try
      {
        tokenHandler.ValidateToken(token, TokenUtil.CreateTokenValidationParameters(Configuration), out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenValidationException();
      }

      if (securityToken == null)
      {
        return false;
      }

      var validatedToken = (JwtSecurityToken)securityToken;
      var userContextInfo = TokenUtil.GetUserContextInfoFromClaims(validatedToken.Claims);
      if (userContextInfo == null)
      {
        return false;
      }

      var userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id);
      if (userConnection == null)
      {
        return false;
      }

      if (userConnection.AccessTokenExpirationDate < DateTimeOffset.Now || userConnection.IsDeleted)
      {
        return false;
      }

      return true;
    }

    public async Task<string> ValidateRecaptchaTokenAsync(string token)
    {
      var ggRecaptchaSetting = Configuration
        .GetSection(AppSettingKey.GG_RECAPTCHA)
        .Get<GGRecaptchaSettingModel>()
        ?? throw new ConfigurationErrorsException();

      Dictionary<string, string?> query = new()
      {
        ["secret"] = ggRecaptchaSetting.SecretKey,
        ["response"] = token
      };
      var uri = QueryHelpers.AddQueryString(ggRecaptchaSetting.Endpoint, query);
      var httpClient = new HttpClient();
      var res = await httpClient.PostAsync(uri, null);

      return await res.Content.ReadAsStringAsync();
    }
  }
}