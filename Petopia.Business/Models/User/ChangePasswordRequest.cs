namespace Petopia.Business.Models.User
{
  public class ChangePasswordRequest
  {
    public string NewPassword { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
  }
}