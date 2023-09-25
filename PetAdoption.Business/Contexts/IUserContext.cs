using PetAdoption.Business.Models.User;
using PetAdoption.Data.Enums;

namespace PetAdoption.Business.Contexts
{
  public interface IUserContext
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
    public string Id { get; set; }
    public string Email { get; set; }
    public void SetUserContext(UserContextModel userInfo);
  }
}