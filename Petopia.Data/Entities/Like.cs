namespace Petopia.Data.Entities
{
	public class Like
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid PostId { get; set; }
		public Guid BlogId { get; set; }
	}
}

