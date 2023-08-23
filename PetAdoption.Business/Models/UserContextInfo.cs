using PetAdoption.Data.Enums;

namespace PetAdoption.Business.Models
{
  public class UserContextInfo
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRole Role { get; set; }
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
  }
}