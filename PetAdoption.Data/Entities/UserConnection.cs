#nullable disable

namespace PetAdoption.Data.Entities
{
  public class UserConnection
  {
    public string Id { get; set; }
    public string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}