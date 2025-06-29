#nullable disable 

namespace Petopia.Data.Entities
{
  public class Post
  {
    public Guid Id { get; set; }
    public Guid UserId { set; get; }
    public Guid CreatorId { get; set; }
    public string Content { set; get; }
    public int Like { get; set; }
    public DateTimeOffset IsCreatedAt { set; get; }
    public bool IsDeleted { set; get; }

    public User User { get; set; }
    public List<Media> Images { get; set; }
    public List<Comment> Comments { get; set; }
  }
}