using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;
using Petopia.Business.Models.Setting;
using Petopia.Business.Models.Exceptions;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class AuthService : BaseService, IAuthService
  {
    private readonly IHttpService _httpService;

    public AuthService(
      IServiceProvider provider,
      ILogger<AuthService> logger,
      IHttpService httpService
    ) : base(provider, logger)
    {
      _httpService = httpService;
    }

    public async Task<AuthenticationResponseModel> LoginAsync(LoginRequestModel request)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == HashUtils.HashString(request.Email));
      if (user != null && HashUtils.VerifyHashedPassword(user.Password, request.Password))
      {
        return await LoginAsync(new UserContextModel()
        {
          Id = user.Id,
          Email = request.Email,
          Role = user.Role
        });
      }
      throw new InvalidCredentialException();
    }

    public async Task<AuthenticationResponseModel> LoginAsync(UserContextModel model)
    {
      var userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == model.Id);
      var accessToken = TokenUtils.CreateAccessToken(model, Configuration);
      var expirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS);
      if (userConnection == null)
      {
        userConnection = await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
        {
          Id = model.Id,
          AccessToken = accessToken,
          AccessTokenExpirationDate = expirationDate
        });
      }
      else
      {
        userConnection.AccessToken = accessToken;
        userConnection.AccessTokenExpirationDate = expirationDate;
        userConnection.IsDeleted = false;
      }
      await UnitOfWork.SaveChangesAsync();
      return new AuthenticationResponseModel()
      {
        AccessToken = accessToken,
        AccessTokenExpirationDate = expirationDate
      };
    }

    public async Task<bool> LogoutAsync()
    {
      var userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstOrDefaultAsync(u => u.Id == UserContext.Id);
      if (userConnection == null)
      {
        return false;
      }
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
        tokenHandler.ValidateToken(token, TokenUtils.CreateTokenValidationParameters(Configuration), out securityToken);
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
      var userContextInfo = TokenUtils.GetUserContextInfoFromClaims(validatedToken.Claims);
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
        throw new SecurityTokenExpiredException();
      }
      return true;
    }

    public async Task<GoogleUserModel> ValidateGoogleLoginTokenAsync(string token)
    {
      var endpoint = Configuration
        .GetSection(AppSettingKey.GG_AUTHENTICATION_ENDPOINT)
        .Get<string>()
        ?? throw new ConfigurationErrorsException();
      var result = await _httpService.GetAsync<GoogleUserModel>(endpoint, new Dictionary<string, string?>()
      {
        ["access_token"] = token,
      });
      if (result == null || !result.Error.IsNullOrEmpty())
      {
        throw new InvalidGoogleTokenException();
      }
      return result;
    }

    public async Task ValidateGoogleRecaptchaTokenAsync(string token)
    {
      var googleRecaptchaSetting = Configuration
        .GetSection(AppSettingKey.GG_RECAPTCHA)
        .Get<GoogleRecaptchaSettingModel>()
        ?? throw new ConfigurationErrorsException();
      var result = await _httpService.GetAsync<GoogleRecaptchaValidationModel>(googleRecaptchaSetting.Endpoint, new Dictionary<string, string?>()
      {
        ["secret"] = googleRecaptchaSetting.SecretKey,
        ["response"] = token
      });
      if (result == null || !result.Success)
      {
        throw new SecurityTokenValidationException();
      }
    }
  }
}