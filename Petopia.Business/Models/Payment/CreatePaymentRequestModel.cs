namespace Petopia.Business.Models.Payment
{
  public class CreatePaymentRequestModel
  {
    public Guid BlogId { get; set; }
    public Guid AdvertisementId { get; set; }
    public string Nonce { get; set; } = null!;
  }
}