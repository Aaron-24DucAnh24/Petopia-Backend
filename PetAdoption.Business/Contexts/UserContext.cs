using PetAdoption.Business.Models.User;
using PetAdoption.Data.Enums;

namespace PetAdoption.Business.Contexts
{
  public class UserContext : IUserContext
  {
    private string _id = "";
    private string _firstName = "";
    private string _lastName = "";
    private UserRole _role;
    private string _email = "";

    public string FirstName { get { return _firstName; } set { _firstName = value; } }
    public string LastName { get { return _lastName; } set { _lastName = value; } }
    public UserRole Role { get { return _role; } set { _role = value; } }
    public string Id { get { return _id; } set { _id = value; } }
    public string Email { get { return _email; } set { _email = value; } }

    public void SetUserContext(UserContextModel userContextInfo)
    {
      Email = userContextInfo.Email;
      FirstName = userContextInfo.FirstName;
      Role = userContextInfo.Role;
      Id = userContextInfo.Id;
      LastName = userContextInfo.LastName;
    }
  }
}