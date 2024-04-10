#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
	public class Blog
	{
		public Guid Id { get; set; }
		public string Excerpt { get; set; }
		public string Image { get; set; }
		public BlogCategory Category { get; set; }
		public string Content { get; set; }
		public bool IsAdvertizing { get; set; }
	}
}