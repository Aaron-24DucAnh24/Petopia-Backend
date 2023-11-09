namespace Petopia.Business.Models.User
{
  public class ResetPasswordRequestModel
  {
    public string Email {get; set;} = null!;
    public string ResetPasswordToken {get; set;} = null!;
    public string Password {get; set;} = null!;
  }
}