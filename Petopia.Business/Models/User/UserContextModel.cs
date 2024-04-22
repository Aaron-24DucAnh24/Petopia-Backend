using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class UserContextModel
  {
    public UserRole Role { get; set; }
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
	}
}