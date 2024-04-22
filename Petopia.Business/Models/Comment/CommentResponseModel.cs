namespace Petopia.Business.Models.Comment
{
	public class CommentResponseModel
	{
		public Guid Id { get; set; }
		public string Content { get; set; } = null!;
		public Guid UserId { get; set; }
		public string UserName { get; set; } = null!;
		public string UserImage { get; set; } = null!;
		public DateTimeOffset IsCreatedAt { get; set; }
	}
}

