#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
	public class Notification	{
		public Guid Id { get; set; }
		public Guid GoalId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public bool IsChecked { get; set; }
		public NotificationType Type { get; set; }
		public Guid UserId { get; set; }

		public User User { get; set; }
	}
}

