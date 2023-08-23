using PetAdoption.Data.Enums;

namespace PetAdoption.Data.Entities
{
  public class User
  {
    public string Id { get; set; } = null!;
    public string FirstName { get; set;} = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; }
  }
}