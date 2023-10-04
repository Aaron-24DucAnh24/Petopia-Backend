using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Constants;
using Petopia.Business.Models.Setting;
using Petopia.Business.Models.User;
using Petopia.Data.Enums;

namespace Petopia.Business.Utils
{
  public static class TokenUtils
  {
    public static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
    {
      var tokenSetting = configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>()
        ?? throw new ConfigurationErrorsException();
      var signingKeyBytes = Encoding.UTF8.GetBytes(tokenSetting.Key);
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

    public static string CreateAccessToken(UserContextModel user, IConfiguration configuration)
    {
      var claims = new List<Claim>()
      {
        new Claim(ClaimType.ID, user.Id),
        new Claim(ClaimType.EMAIL, user.Email),
        new Claim(ClaimType.ROLE, user.Role.ToString())
      };
      var tokenSetting = configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>()
        ?? throw new ConfigurationErrorsException();
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSetting.Key));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken
      (
        tokenSetting.Issuer,
        tokenSetting.Issuer,
        claims,
        signingCredentials: creds,
        expires: DateTime.Now.AddDays(TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS),
        notBefore: DateTime.Now
      );
      var tokenHandler = new JwtSecurityTokenHandler();
      return tokenHandler.WriteToken(token);
    }

    public static string CreateSecurityToken()
    {
      var bytes = new byte[64];
      (new Random()).NextBytes(bytes);
      return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_");
    }

    public static string? GetAccessTokenFromRequest(HttpRequest request)
    {
      var bearerToken = request.Headers["Authorization"].FirstOrDefault();
      if (bearerToken == null)
      {
        return null;
      }
      var jwtToken = bearerToken[(JwtBearerDefaults.AuthenticationScheme.Length + 1)..].Trim();
      return jwtToken;
    }

    public static UserContextModel? GetUserContextInfoFromClaims(IEnumerable<Claim> claims)
    {
      var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimType.EMAIL);
      var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ROLE);
      var idClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ID);
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
        Id = idClaim.Value
      };
    } 
  }
}