namespace Petopia.Business.Models.Post
{
	public class PostResponseModel
	{
		public Guid Id { get; set; }
		public Guid CreatorId { get; set; }
		public string Content { set; get; } = null!;
		public string UserImage { set; get; } = null!;
		public string UserName { set; get; } = null!;
		public int Like { get; set; }
		public int CommentCount { get; set; }
		public bool IsLiked { get; set; }
		public DateTimeOffset IsCreatedAt { set; get; }
		public List<string> Images { get; set; } = null!;
	}
}

