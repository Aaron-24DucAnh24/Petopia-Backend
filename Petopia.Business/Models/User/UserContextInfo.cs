using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class UserContextModel
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRole Role { get; set; }
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
  }
}