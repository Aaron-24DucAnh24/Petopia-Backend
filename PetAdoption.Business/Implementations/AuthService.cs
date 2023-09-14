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
        throw new Exception(ExceptionMessage.DUPLICATE);
      }

      User user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Password = request.Password,
      });

      UserConnection userConnection = await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
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
      User user = await UnitOfWork.Users
        .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password)
        ?? throw new Exception(ExceptionMessage.INCORRECT_LOGIN_INFO);

      UserConnection userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .Where(u => u.Id == user.Id)
        .FirstOrDefaultAsync()
        ?? throw new Exception(ExceptionMessage.INCORRECT_LOGIN_INFO);

      string accessToken = TokenUtil.GenerateAccessToken(user, Configuration);
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

    public async Task<bool> LogoutAsync()
    {
      UserConnection userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstOrDefaultAsync(u => u.Id == UserContext.Id)
        ?? throw new Exception("UserConnection not found");

      userConnection.IsDeleted = true;
      await UnitOfWork.SaveChangesAsync();
      
      return true;
    }

    public bool ValidateAccessToken(string token)
    {
      JwtSecurityTokenHandler tokenHandler = new();

      SecurityToken? securityToken;
      try
      {
        tokenHandler.ValidateToken(token, TokenUtil.CreateTokenValidationParameters(Configuration), out securityToken);
      }
      catch (Exception)
      {
        throw new Exception(ExceptionMessage.INVALID_ACCESS_TOKEN);
      }

      if (securityToken == null)
      {
        return false;
      }

      JwtSecurityToken validatedToken = (JwtSecurityToken)securityToken;
      UserContextModel? userContextInfo = TokenUtil.GetUserContextInfo(validatedToken.Claims);
      if (userContextInfo == null)
      {
        return false;
      }

      UserConnection? userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id);
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
      string endpoint = Configuration.GetValue<string>("GoogleRecaptcha:Endpoint")
        ?? throw new Exception("Recaptcha configuration not found");
      string securityToken = Configuration.GetValue<string>("GoogleRecaptcha:SecurityToken")
        ?? throw new Exception("Recaptcha configuration not found");

      Dictionary<string, string?> query = new()
      {
        ["secret"] = securityToken,
        ["response"] = token
      };
      string? uri = QueryHelpers.AddQueryString(endpoint, query);
      HttpClient httpClient = new();
      HttpResponseMessage res = await httpClient.PostAsync(uri, null);

      return await res.Content.ReadAsStringAsync();
    }
  }
}