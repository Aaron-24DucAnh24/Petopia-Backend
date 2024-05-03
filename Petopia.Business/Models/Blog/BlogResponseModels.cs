using Petopia.Data.Enums;

namespace Petopia.Business.Models.Blog
{
  public class BlogDetailResponseModel
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public string Image { get; set; } = null!;
    public BlogCategory Category { get; set; }
    public string Content { get; set; } = null!;
  }

  public class BlogResponseModel
  {
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public string Image { get; set; } = null!;
    public BlogCategory Category { get; set; }
    public string UserName { get; set; } = null!;
  }
}

