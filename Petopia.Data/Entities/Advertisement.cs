#nullable disable

namespace Petopia.Data.Entities
{
  public class Advertisement
  {
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int MonthDuration { get; set; }
  }
}