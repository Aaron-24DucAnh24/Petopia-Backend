using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
  public class CookieService : BaseService, ICookieService
  {
    public CookieService(
      IServiceProvider provider,
      ILogger<CookieService> logger
    ) : base(provider, logger)
    {
    }

    public void ClearJwtTokens()
    {
      SetCookie(CookieName.ACCESS_TOKEN, string.Empty, -1);
      SetCookie(CookieName.REFRESH_TOKEN, string.Empty, -1, true);
    }

    public void SetJwtTokens(string accessToken, string refreshToken)
    {
      SetCookie(CookieName.ACCESS_TOKEN, accessToken, TokenSettingConstants.ACCESS_TOKEN_EXPIRATION_DAYS);
      SetCookie(CookieName.REFRESH_TOKEN, refreshToken, TokenSettingConstants.REFRESH_TOKEN_EXPIRATION_DAYS, true);
    }

    private void SetCookie(string key, string value, double expirationDays, bool httpOnly = false)
    {
      HttpContextAccessor?.HttpContext?.Response.Cookies.Append(key, value, new CookieOptions()
      {
        Expires = DateTimeOffset.Now.AddDays(expirationDays),
        HttpOnly = httpOnly,
        Secure = true,
        SameSite = SameSiteMode.None,
      });
    }
  }
}