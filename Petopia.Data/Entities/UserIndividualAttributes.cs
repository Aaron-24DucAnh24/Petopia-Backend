#nullable disable

namespace Petopia.Data.Entities
{
  public class UserIndividualAttributes
  {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public User User { get; set; }
  }
}