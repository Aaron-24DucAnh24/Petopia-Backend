namespace Petopia.Business.Interfaces
{
  public interface ICookieService
  {
    public void ClearJwtTokens();
    public void SetJwtTokens(string accessToken, string refreshToken);
  }
}