#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Media
  {
    public Guid Id { get; set; }
    public Guid? PetId { get; set; }
    public Guid? PostId { get; set; }
    public MediaType Type { get; set; }
    public string Url { get; set; }

    public virtual Pet Pet { get; set; }
    public virtual Post Post { get; set; }
	}
}
