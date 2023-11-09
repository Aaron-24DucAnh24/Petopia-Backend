namespace Petopia.Business.Models.User
{
  public class ChangePasswordRequestModel
  {
    public string NewPassword { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
  }
}