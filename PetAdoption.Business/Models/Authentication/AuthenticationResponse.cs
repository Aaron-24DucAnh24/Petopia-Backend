namespace PetAdoption.Business.Models
{
  public class AuthenticationResponse
  {
    public string AccessToken { get; set; } = null!;
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
  }
}