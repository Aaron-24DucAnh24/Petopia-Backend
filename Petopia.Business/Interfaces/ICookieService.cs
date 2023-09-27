namespace Petopia.Business.Interfaces
{
  public interface ICookieService
  {
    public void ClearAccessToken();
    public void SetAccessToken(string token);
  }
}