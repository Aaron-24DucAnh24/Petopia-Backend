using Microsoft.AspNetCore.Http;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Services.Interfaces;

namespace PetAdoption.Business.Services.Implementations
{
  public class CookieService : BaseService, ICookieService
  {
    public CookieService(IServiceProvider provider) : base(provider)
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