namespace Petopia.Business.Models.Post
{
  public class CreatePostRequestModel
  {
    public Guid PetId { set; get; }
    public string Content { set; get; } = null!;
    public List<string> Images { get; set; } = null!;
  }
}

