namespace Petopia.Business.Models.Authentication
{
  public class LoginRequestModel
  {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
  }
}