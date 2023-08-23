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

    string IUserContext.FirstName { get { return _firstName; } set { _firstName =  value; } }
    string IUserContext.LastName { get { return _lastName; } set { _lastName =  value; } }
    UserRole IUserContext.Role { get { return _role; } set { _role =  value; } }
    string IUserContext.Id { get { return _id; } set { _id =  value; } }
    string IUserContext.Email { get { return _email; } set { _email =  value; } }
  }
}