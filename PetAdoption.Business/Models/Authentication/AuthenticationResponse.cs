namespace PetAdoption.Business.Models.Authentication
{
  public class AuthenticationResponse
  {
    public string AccessToken { get; set; } = null!;
    public DateTimeOffset AccessTokenExpirationDate { get; set; }
  }
}