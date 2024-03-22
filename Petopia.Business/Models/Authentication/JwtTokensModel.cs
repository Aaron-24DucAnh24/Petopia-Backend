namespace Petopia.Business.Models.Authentication
{
  public class JwtTokensModel
  {
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTimeOffset AccessTokenExpiredDate { get; set; }
    public DateTimeOffset RefreshTokenExpiredDate { get; set; }
  }
}