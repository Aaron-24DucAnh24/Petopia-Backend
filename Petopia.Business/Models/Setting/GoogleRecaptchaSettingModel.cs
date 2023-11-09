namespace Petopia.Business.Models.Setting
{
  public class GoogleRecaptchaSettingModel
  {
    public string Endpoint { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string SiteKey { get; set; } = null!;
  }
}