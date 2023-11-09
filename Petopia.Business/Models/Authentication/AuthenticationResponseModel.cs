namespace Petopia.Business.Models.Authentication
{
  public class JwtTokensModel
  {
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
  }
}