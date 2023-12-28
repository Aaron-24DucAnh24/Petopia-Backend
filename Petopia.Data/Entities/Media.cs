#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Media
  {
    public Guid Id { get; set; }
    public MediaType Type { get; set; }
    public string Url { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSynced { get; set; }

    public Guid PetId { get; set; }
    public Pet Pet { get; set; }
  }
}
