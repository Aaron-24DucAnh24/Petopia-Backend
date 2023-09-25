using System.Text.Json.Serialization;

namespace PetAdoption.Business.Models.User
{
  public class GoogleUserInfo
  {
    [JsonPropertyName("given_name")]
    public string GivenName { get; set; } = string.Empty;

    [JsonPropertyName("family_name")]
    public string FamilyName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = null!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("locale")]
    public string Locale { get; set; } = null!;

    [JsonPropertyName("error")]
    public string Error {get; set;} = string.Empty;
  }
}