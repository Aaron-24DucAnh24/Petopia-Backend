using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Constants;
using Petopia.Business.Models.Enums;
using Petopia.Business.Models.Setting;
using Petopia.Business.Models.User;
using Petopia.Data.Enums;

namespace Petopia.Business.Utils
{
  public static class TokenUtils
  {
    public static string CreateAccessToken(UserContextModel user, IConfiguration configuration)
    {
      return CreateJwtToken(TokenType.AccessToken, user, configuration);
    }

    public static string CreateRefreshToken(UserContextModel user, IConfiguration configuration)
    {
      return CreateJwtToken(TokenType.RefreshToken, user, configuration);
    }

    public static string CreateSecurityToken()
    {
      var bytes = new byte[64];
      (new Random()).NextBytes(bytes);
      return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_");
    }

    public static TokenValidationParameters CreateTokenValidationParameters(TokenType type, IConfiguration configuration)
    {
      TokenSettingModel tokenSetting = configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>()
        ?? throw new ConfigurationErrorsException();
      string secretKey = type == TokenType.AccessToken ? tokenSetting.AccessTokenKey : tokenSetting.RefreshTokenKey;
      byte[] signingKeyBytes = Encoding.UTF8.GetBytes(secretKey);
      return new TokenValidationParameters()
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenSetting.Issuer,
        ValidAudience = tokenSetting.Issuer,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
      };
    }

    public static string? GetAccessTokenFromRequest(HttpRequest request)
    {
      string? bearerToken = request.Headers["Authorization"].FirstOrDefault();
      string? jwtToken = string.IsNullOrEmpty(bearerToken)
        ? request.Cookies[CookieName.ACCESS_TOKEN]
        : bearerToken[(JwtBearerDefaults.AuthenticationScheme.Length + 1)..].Trim();
      return jwtToken;
    }

    public static UserContextModel? GetUserContextInfoFromClaims(IEnumerable<Claim> claims)
    {
      Claim? emailClaim = claims.FirstOrDefault(c => c.Type == ClaimType.EMAIL);
      Claim? roleClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ROLE);
      Claim? idClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ID);
      if (emailClaim == null
      || roleClaim == null
      || emailClaim == null
      || idClaim == null)
      {
        return null;
      }
      return new UserContextModel()
      {
        Email = emailClaim.Value,
        Role = (UserRole)Enum.Parse(typeof(UserRole), roleClaim.Value),
        Id = new Guid(idClaim.Value)
      };
    }

    private static string CreateJwtToken(TokenType type, UserContextModel user, IConfiguration configuration)
    {
      TokenSettingModel tokenSetting = configuration.GetRequiredSection(AppSettingKey.TOKEN).Get<TokenSettingModel>()
        ?? throw new ConfigurationErrorsException();
      List<Claim> claims = new()
      {
        new Claim(ClaimType.ID, user.Id.ToString()),
        new Claim(ClaimType.EMAIL, user.Email),
        new Claim(ClaimType.ROLE, user.Role.ToString())
      };
      string secretKey = type == TokenType.AccessToken ? tokenSetting.AccessTokenKey : tokenSetting.RefreshTokenKey;
      double expirationDays = type == TokenType.AccessToken ? TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS : TokenSettingConstants.REFRESH_TOKEN_EXPIRATION_DAYS;
      SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKey));
      SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
      JwtSecurityToken token = new
      (
        tokenSetting.Issuer,
        tokenSetting.Issuer,
        claims: claims,
        signingCredentials: creds,
        expires: DateTime.Now.AddDays(expirationDays),
        notBefore: DateTime.Now
      );
      return (new JwtSecurityTokenHandler()).WriteToken(token);
    }
  }
}