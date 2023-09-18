using PetAdoption.Data.Enums;

#nullable disable

namespace PetAdoption.Data.Entities
{
  public class User
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
  }
}