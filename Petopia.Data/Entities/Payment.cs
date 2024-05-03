#nullable disable

namespace Petopia.Data.Entities
{
  public class Payment
  {
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public DateTimeOffset AdvertisingDate { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public int Amount { get; set; }

    public Blog Blog { get; set; }
  }
}