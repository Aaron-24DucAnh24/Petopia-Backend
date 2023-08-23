namespace PetAdoption.Data.Entities
{
    public class UserConnection
  {
    public string Id { get; set; } = null!;    
    public string AccessToken { get; set; } = null!;
    public DateTimeOffset AccessTokenExpirationDate { get; set;}
    public bool IsDeleted { get; set; }
  }
}