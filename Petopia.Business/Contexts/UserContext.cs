using Petopia.Business.Models.User;
using Petopia.Data.Enums;

namespace Petopia.Business.Contexts
{
  public class UserContext : IUserContext
  {
    private Guid _id;
    private UserRole _role;
    private string _email = "";
    public UserRole Role { get { return _role; } set { _role = value; } }
    public Guid Id { get { return _id; } set { _id = value; } }
    public string Email { get { return _email; } set { _email = value; } }

    public void SetUserContext(UserContextModel userContextInfo)
    {
      Email = userContextInfo.Email;
      Role = userContextInfo.Role;
      Id = userContextInfo.Id;
    }
  }
}