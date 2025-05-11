using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
      SetCookie(Constants.COOKIES_NAME_ACCESS_TOKEN, string.Empty, -1);
      SetCookie(Constants.COOKIES_NAME_REFRESH_TOKEN, string.Empty, -1);
    }

    public void SetJwtTokens(string accessToken, string refreshToken)
    {
      SetCookie(Constants.COOKIES_NAME_ACCESS_TOKEN, accessToken, Constants.TOKEN_SETTING_ACCESS_TOKEN_EXPIRATION_DAYS);
      SetCookie(Constants.COOKIES_NAME_REFRESH_TOKEN, refreshToken, Constants.TOKEN_SETTING_REFRESH_TOKEN_EXPIRATION_DAYS);
    }

    private void SetCookie(string key, string value, double expirationDays)
    {
      HttpContextAccessor?.HttpContext?.Response.Cookies.Append(key, value, new CookieOptions()
      {
        Expires = DateTimeOffset.Now.AddDays(expirationDays),
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
      });
    }
  }
}