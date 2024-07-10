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
using Petopia.Business.Models.Enums;
using StackExchange.Redis;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class AuthService : BaseService, IAuthService
  {
    private const string ACCESS_TOKEN = "access_token";
    private const string RESPONSE = "response";
    private const string SECRET = "secret";

    public AuthService(
      IServiceProvider provider,
      ILogger<AuthService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<JwtTokensModel> LoginAsync(LoginRequestModel request)
    {
      User user = await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == HashUtils.EnryptString(request.Email))
        ?? throw new InvalidCredentialException();
      if (string.IsNullOrEmpty(user.Password))
      {
        throw new WrongLoginMethodException();
      }
      if (!HashUtils.VerifyHashedPassword(user.Password, request.Password))
      {
        throw new InvalidCredentialException();
      }
      return await LoginAsync(new UserContextModel()
      {
        Id = user.Id,
        Email = request.Email,
        Role = user.Role
      });
    }

    public async Task<JwtTokensModel> LoginAsync(UserContextModel model)
    {
      UserConnection? userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == model.Id);
      string accessToken = TokenUtils.CreateAccessToken(model, Configuration);
      string refreshToken = TokenUtils.CreateRefreshToken(model, Configuration);
      DateTimeOffset accessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS);
      DateTimeOffset refreshTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
      if (userConnection == null)
      {
        userConnection = await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
        {
          Id = model.Id,
          AccessToken = accessToken,
          RefreshToken = refreshToken,
          AccessTokenExpirationDate = accessTokenExpirationDate,
          RefreshTokenExpirationDate = refreshTokenExpirationDate
        });
      }
      else
      {
        userConnection.AccessToken = accessToken;
        userConnection.RefreshToken = refreshToken;
        userConnection.AccessTokenExpirationDate = accessTokenExpirationDate;
        userConnection.RefreshTokenExpirationDate = refreshTokenExpirationDate;
        userConnection.IsDeleted = false;
      }
      await UnitOfWork.SaveChangesAsync();
      return new JwtTokensModel()
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        RefreshTokenExpiredDate = userConnection.RefreshTokenExpirationDate,
        AccessTokenExpiredDate = userConnection.AccessTokenExpirationDate,
      };
    }

    public async Task<bool> LogoutAsync()
    {
      UserConnection userConnection = await UnitOfWork.UserConnections
        .AsTracking()
        .FirstAsync(u => u.Id == UserContext.Id);
      userConnection.IsDeleted = true;
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<string> CacheRegisterRequestAsync(RegisterRequestModel request)
    {
      if (await UnitOfWork.Users.AnyAsync(x => x.Email == HashUtils.EnryptString(request.Email)))
      {
        throw new UsedEmailException();
      }
      string cacheKey = TokenUtils.CreateSecurityToken();
      CacheManager.Instance.Set(cacheKey, request, TokenSettingConstants.REGISTER_TOKEN_EXPIRATION_DAYS);
      return cacheKey;
    }

    public UserContextModel ValidateAccessToken(string token)
    {
      JwtSecurityTokenHandler tokenHandler = new();
      SecurityToken? securityToken;
      try
      {
        TokenValidationParameters parameters = TokenUtils.CreateTokenValidationParameters(TokenType.AccessToken, Configuration);
        tokenHandler.ValidateToken(token, parameters, out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenValidationException();
      }
      JwtSecurityToken validatedToken = (JwtSecurityToken)securityToken
        ?? throw new SecurityTokenValidationException();
      UserContextModel userContextInfo = TokenUtils.GetUserContextInfoFromClaims(validatedToken.Claims)
        ?? throw new SecurityTokenValidationException();
      UserConnection userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id && x.AccessToken == token)
        ?? throw new SecurityTokenValidationException();
      if (userConnection.AccessTokenExpirationDate < DateTimeOffset.Now || userConnection.IsDeleted)
      {
        throw new SecurityTokenExpiredException();
      }
      return userContextInfo;
    }

    public UserContextModel ValidateRefreshToken(string? token)
    {
      if (string.IsNullOrEmpty(token))
      {
        token = HttpContextAccessor.HttpContext?.Request.Cookies[CookieName.REFRESH_TOKEN]
          ?? throw new SecurityTokenValidationException();
      }
      JwtSecurityTokenHandler tokenHandler = new();
      SecurityToken? securityToken;
      try
      {
        TokenValidationParameters parameters = TokenUtils.CreateTokenValidationParameters(TokenType.RefreshToken, Configuration);
        tokenHandler.ValidateToken(token, parameters, out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenValidationException();
      }
      JwtSecurityToken validatedToken = (JwtSecurityToken)securityToken
        ?? throw new SecurityTokenValidationException();
      UserContextModel userContextInfo = TokenUtils.GetUserContextInfoFromClaims(validatedToken.Claims)
        ?? throw new SecurityTokenValidationException();
      UserConnection userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id && x.RefreshToken == token)
        ?? throw new SecurityTokenValidationException();
      if (userConnection.RefreshTokenExpirationDate < DateTimeOffset.Now || userConnection.IsDeleted)
      {
        throw new SecurityTokenExpiredException();
      }
      return userContextInfo;
    }

    public async Task<GoogleUserModel> ValidateGoogleLoginTokenAsync(string token)
    {
      GoogleAuthSettingModel configs = Configuration
        .GetSection(AppSettingKey.GG_AUTH)
        .Get<GoogleAuthSettingModel>()
        ?? throw new ConfigurationErrorsException();
      GoogleUserModel? result = await HttpService.GetAsync<GoogleUserModel>(configs.Endpoint, new Dictionary<string, string?>()
      {
        [ACCESS_TOKEN] = token,
      });
      if (result == null || !result.Error.IsNullOrEmpty())
      {
        throw new InvalidGoogleTokenException();
      }
      return result;
    }

    public async Task ValidateGoogleRecaptchaTokenAsync(string token)
    {
      GoogleRecaptchaSettingModel googleRecaptchaSetting = Configuration
        .GetSection(AppSettingKey.GG_RECAPTCHA)
        .Get<GoogleRecaptchaSettingModel>()
        ?? throw new ConfigurationErrorsException();
      GoogleRecaptchaValidationModel? result = await HttpService.GetAsync<GoogleRecaptchaValidationModel>(googleRecaptchaSetting.Endpoint, new Dictionary<string, string?>()
      {
        [SECRET] = googleRecaptchaSetting.SecretKey,
        [RESPONSE] = token
      });
      if (result == null || !result.Success)
      {
        throw new GoogleRecaptchaTokenException();
      }
    }

    public string GetGoogleRecaptchaSiteKey()
    {
      GoogleRecaptchaSettingModel googleRecaptchaSetting = Configuration
        .GetSection(AppSettingKey.GG_RECAPTCHA)
        .Get<GoogleRecaptchaSettingModel>()
        ?? throw new ConfigurationErrorsException();
      return googleRecaptchaSetting.SiteKey;
    }

    public string GetGoogleAuthClientId()
    {
      GoogleAuthSettingModel googleAuthSetting = Configuration
        .GetSection(AppSettingKey.GG_AUTH)
        .Get<GoogleAuthSettingModel>()
        ?? throw new ConfigurationErrorsException();
      return googleAuthSetting.ClientId;
    }

    public async Task<JwtTokensModel> AdminLoginAsync(LoginRequestModel request)
    {
      User user = await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == HashUtils.EnryptString(request.Email))
        ?? throw new InvalidCredentialException();
      if (user.Role != UserRole.SystemAdmin)
      {
        throw new InvalidCredentialException();
      }
      if (string.IsNullOrEmpty(user.Password))
      {
        throw new InvalidCredentialException();
      }
      if (!HashUtils.VerifyHashedPassword(user.Password, request.Password))
      {
        throw new InvalidCredentialException();
      }
      return await LoginAsync(new UserContextModel()
      {
        Id = user.Id,
        Email = request.Email,
        Role = user.Role
      });
    }
  }
}