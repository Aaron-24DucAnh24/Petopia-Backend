#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Blog
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Excerpt { get; set; }
    public string Image { get; set; }
    public string Content { get; set; }
    public int View { get; set; }
    public int Like { get; set; }
    public bool IsHidden { get; set; }
    public BlogCategory Category { get; set; }
    public DateTimeOffset AdvertisingDate { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset IsUpdatedAt { get; set; }

    public User User { get; set; }
    public List<Comment> Comments { get; set; }
    public Payment Payment { get; set; }
    public List<Report> Reports { get; set; }
  }
}