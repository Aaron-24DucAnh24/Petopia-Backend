namespace Petopia.Business.Models.Post
{
  public class CreatePostRequestModel
  {
    public string Content { set; get; } = null!;
    public List<string> Images { get; set; } = null!;
  }
}

