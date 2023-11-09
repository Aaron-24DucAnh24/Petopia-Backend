namespace Petopia.Business.Models.Setting
{
  public class TokenSettingModel
  {
    public string AccessTokenKey { get; set; } = null!;
    public string RefreshTokenKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
  }
}