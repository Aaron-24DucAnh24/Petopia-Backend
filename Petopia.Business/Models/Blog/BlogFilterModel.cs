using Petopia.Data.Enums;

namespace Petopia.Business.Models.Blog
{
  public class BlogFilterModel
  {
    public List<BlogCategory>? Category { get; set; }
    public List<Guid>? UserId { get; set; }
  }
}