namespace Petopia.Business.Models.Authentication
{
  public class ValidateRegisterRequestModel
  {
    public string Email { get; set;} = null!;
    public string ValidateRegisterToken { get; set;} = null!;
  }
}