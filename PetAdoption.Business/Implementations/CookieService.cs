using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class CookieService : BaseService, ICookieService
  {
    public CookieService(
      IServiceProvider provider, 
      ILogger<CookieService> logger
    ) : base(provider, logger)
    {
    }


    public void ClearAccessToken()
    {
      SetCookie(CookieName.ACCESS_TOKEN, string.Empty, -1);
    }

    public void SetAccessToken(string token)
    {
      SetCookie(CookieName.ACCESS_TOKEN, token, TokenSetting.ACCESS_TOKEN_EXPIRATION_DAYS);
    }

    private void SetCookie(string key, string value, int expirationDays)
    {
      if(HttpContextAccessor.HttpContext == null)
        throw new Exception("HttpContext Not Found");

      HttpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions()
      {
        Expires = DateTimeOffset.Now.AddDays(expirationDays)
      });
    }
  }
}