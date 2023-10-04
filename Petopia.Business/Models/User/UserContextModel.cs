using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class UserContextModel
  {
    public UserRole Role { get; set; }
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
  }
}