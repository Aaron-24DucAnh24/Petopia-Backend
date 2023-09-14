using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Models;
using PetAdoption.Data.Entities;
using PetAdoption.Data.Enums;

namespace PetAdoption.Business.Utils
{
  public static class TokenUtil
  {
    public static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
    {
      var tokenSetting = configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>() 
        ?? throw new Exception("Token configuration not found");

      byte[] signingKeyBytes = Encoding.UTF8.GetBytes(tokenSetting.Key);

      return new TokenValidationParameters()
      {
        ValidateIssuer = true,
        ValidIssuer = tokenSetting.Issuer,
        ValidateAudience = true,
        ValidAudience = tokenSetting.Issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
      };
    }

    public static string? GetAccessTokenFromRequest(HttpRequest request)
    {
      string? bearerToken = request.Headers["Authorization"].FirstOrDefault();
      if (bearerToken == null)
      {
        return null;
      }
      string jwtToken = bearerToken[(JwtBearerDefaults.AuthenticationScheme.Length + 1)..].Trim();
      return jwtToken;
    }

    public static UserContextModel? GetUserContextInfo(IEnumerable<Claim> claims)
    {
      Claim? emailClaim = claims.FirstOrDefault(c => c.Type == ClaimType.EMAIL);
      Claim? firstNameClaim = claims.FirstOrDefault(c => c.Type == ClaimType.FIRST_NAME);
      Claim? lastNameClaim = claims.FirstOrDefault(c => c.Type == ClaimType.LAST_NAME);
      Claim? roleClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ROLE);
      Claim? idClaim = claims.FirstOrDefault(c => c.Type == ClaimType.ID);

      if (emailClaim == null
      || firstNameClaim == null
      || roleClaim == null
      || lastNameClaim == null
      || emailClaim == null
      || idClaim == null)
      {
        return null;
      }

      return new UserContextModel()
      {
        FirstName = firstNameClaim.Value,
        LastName = lastNameClaim.Value,
        Email = emailClaim.Value,
        Role = (UserRole)Enum.Parse(typeof(UserRole), roleClaim.Value),
        Id = idClaim.Value
      };
    }

    public static string GenerateAccessToken(User user, IConfiguration configuration)
    {
      List<Claim> claims = new()
      {
        new Claim(ClaimType.ID, user.Id),
        new Claim(ClaimType.EMAIL, user.Email),
        new Claim(ClaimType.ROLE, user.Role.ToString()),
        new Claim(ClaimType.FIRST_NAME, user.FirstName),
        new Claim(ClaimType.LAST_NAME, user.LastName)
      };

      var tokenSetting = configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>() 
        ?? throw new Exception("Token configuration not found");

      SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(tokenSetting.Key));
      SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
      JwtSecurityToken token = new(
        tokenSetting.Issuer,
        tokenSetting.Issuer,
        claims,
        signingCredentials: creds,
        expires: DateTime.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS),
        notBefore: DateTime.Now
      );

      JwtSecurityTokenHandler tokenHandler = new();
      return tokenHandler.WriteToken(token);
    }
  }
}