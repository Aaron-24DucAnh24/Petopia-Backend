namespace Petopia.Business.Models.Authentication
{
  public class AuthenticationResponseModel
  {
    public string AccessToken { get; set; } = null!;
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
  }
}