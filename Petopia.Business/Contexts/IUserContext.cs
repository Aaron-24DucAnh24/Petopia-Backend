using Petopia.Business.Models.User;
using Petopia.Data.Enums;

namespace Petopia.Business.Contexts
{
  public interface IUserContext
  {
    public UserRole Role { get; set; }
    public string Id { get; set; }
    public string Email { get; set; }
    public void SetUserContext(UserContextModel userInfo);
  }
}