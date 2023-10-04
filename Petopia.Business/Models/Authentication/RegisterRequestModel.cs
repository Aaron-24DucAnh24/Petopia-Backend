namespace Petopia.Business.Models.Authentication
{
  public class RegisterRequestModel
  {
    public string FirstName { get; set;} = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string GoogleRecaptchaToken {get; set;} = null!;
  }
}