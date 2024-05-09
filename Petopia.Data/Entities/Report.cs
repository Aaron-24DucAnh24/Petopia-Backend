#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Report
  {
    public Guid Id { get; set; }
    public Guid ReporterId { get; set; }
    public ReportType Type { get; set; }
    public Guid? BlogId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? PetId { get; set; }

    public virtual Blog Blog { get; set; }
    public virtual User User { get; set; }
    public virtual Pet Pet { get; set; }
  }
}