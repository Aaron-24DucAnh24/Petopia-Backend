namespace Petopia.Business.Models.Payment
{
  public class CreatePaymentResponseModel
  {
    public Guid PaymentId { get; set; }
    public int Price { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset AdvertisingDate { get; set; }
    public string UserEmail { get; set; } = null!;
    public string Description { get; set; } = null!;
  }
}