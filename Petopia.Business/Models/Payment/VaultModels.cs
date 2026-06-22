#nullable disable

namespace Petopia.Business.Models.Payment
{
  public class SavedCardResponseModel
  {
    public string Token { get; set; }
    public string CardType { get; set; }
    public string Last4 { get; set; }
    public string ExpirationMonth { get; set; }
    public string ExpirationYear { get; set; }
    public string CardholderName { get; set; }
    public string ImageUrl { get; set; }
    public bool IsDefault { get; set; }
  }

  public class VaultCardRequestModel
  {
    public string Nonce { get; set; }
  }
}
