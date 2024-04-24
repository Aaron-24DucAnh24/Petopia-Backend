#nullable disable

namespace Petopia.Data.Entities
{
  public class Comment
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid? BlogId { get; set; }
    public Guid? PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }

    public virtual Post Post { get; set; }
    public virtual Blog Blog { get; set; }
  }
}

