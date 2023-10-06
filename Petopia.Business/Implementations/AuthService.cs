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
      var refreshToken = TokenUtils.CreateRefreshToken(model, Configuration);
      var accessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS);
      var refreshTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
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
      return new AuthenticationResponseModel()
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        AccessTokenExpirationDate = accessTokenExpirationDate,
        RefreshTokenExpirationDate = refreshTokenExpirationDate
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

    public UserContextModel ValidateAccessToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken? securityToken;
      try
      {
        var parameters = TokenUtils.CreateTokenValidationParameters(TokenType.AccessToken, Configuration);
        tokenHandler.ValidateToken(token, parameters, out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenValidationException();
      }
      var validatedToken = (JwtSecurityToken)securityToken;
      if (validatedToken == null)
      {
        throw new SecurityTokenValidationException();
      }
      var userContextInfo = TokenUtils.GetUserContextInfoFromClaims(validatedToken.Claims);
      if (userContextInfo == null)
      {
        throw new SecurityTokenValidationException();
      }
      var userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id);
      if (userConnection == null)
      {
        throw new SecurityTokenValidationException();
      }
      if (userConnection.AccessTokenExpirationDate < DateTimeOffset.Now || userConnection.IsDeleted)
      {
        throw new SecurityTokenExpiredException();
      }
      return userContextInfo;
    }

    public UserContextModel ValidateRefreshToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken? securityToken;
      try
      {
        var parameters = TokenUtils.CreateTokenValidationParameters(TokenType.RefreshToken, Configuration);
        tokenHandler.ValidateToken(token, parameters, out securityToken);
      }
      catch (Exception)
      {
        throw new SecurityTokenValidationException();
      }
      var validatedToken = (JwtSecurityToken)securityToken;
      if (validatedToken == null)
      {
        throw new SecurityTokenValidationException();
      }
      var userContextInfo = TokenUtils.GetUserContextInfoFromClaims(validatedToken.Claims);
      if (userContextInfo == null)
      {
        throw new SecurityTokenValidationException();
      }
      var userConnection = UnitOfWork.UserConnections.FirstOrDefault(x => x.Id == userContextInfo.Id);
      if (userConnection == null)
      {
        throw new SecurityTokenValidationException();
      }
      if (userConnection.RefreshTokenExpirationDate < DateTimeOffset.Now || userConnection.IsDeleted)
      {
        throw new SecurityTokenExpiredException();
      }
      return userContextInfo;
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