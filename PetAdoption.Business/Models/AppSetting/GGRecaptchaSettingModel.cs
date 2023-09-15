namespace PetAdoption.Business.Models
{
  public class GGRecaptchaSettingModel
  {
    public string Endpoint { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string SiteKey { get; set; } = null!;
  }
}