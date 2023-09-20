using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Models;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
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

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
      var user = await UnitOfWork.Users
        .AsTracking()
        .Include(x => x.UserConnection)
        .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password)
        ?? throw new InvalidCredentialException();
      user.UserConnection.AccessToken = TokenUtil.GenerateAccessToken(user, Configuration);
      user.UserConnection.AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS);
      user.UserConnection.IsDeleted = false;
      await UnitOfWork.SaveChangesAsync();
      return new AuthenticationResponse()
      {
        AccessToken = user.UserConnection.AccessToken,
        AccessTokenExpirationDate = user.UserConnection.AccessTokenExpirationDate
      };
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
        throw new SecurityTokenExpiredException();
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