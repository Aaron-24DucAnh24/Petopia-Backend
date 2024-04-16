using Petopia.Data.Enums;

namespace Petopia.Business.Models.Notification
{
	public class NotificationResponseModel
	{
		public Guid Id { get; set; }
		public Guid GoalId { get; set; }
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public bool IsChecked { get; set; }
		public NotificationType Type { get; set; }
		public Guid UserId { get; set; }
		public DateTimeOffset IsCreatedAt { get; set; }
	}
}

