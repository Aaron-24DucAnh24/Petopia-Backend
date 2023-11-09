namespace Petopia.Business.Models.Setting
{
  public class BraintreeSettingModel
  {
    public bool IsProduction { get; set; }
    public string MerchantId { get; set; } = null!;
    public string PublicKey { get; set; } = null!;
    public string PrivateKey { get; set; } = null!;
  }
}