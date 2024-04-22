namespace Petopia.Business.Models.Comment
{
	public class CreateCommentRequestModel
	{
		public string Content { get; set; } = null!;
		public Guid? BlogId { get; set; }
		public Guid? PostId { get; set; }
	}
}