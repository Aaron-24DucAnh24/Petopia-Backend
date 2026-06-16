using Petopia.Data.Enums;

namespace Petopia.Business.Models.Blog
{
  public class BlogFilterModel
  {
    public List<BlogCategory>? Category { get; set; }
    public Guid? UserId { get; set; }
    public string? Text { get; set; }
  }
}