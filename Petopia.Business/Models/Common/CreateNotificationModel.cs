using Petopia.Data.Enums;

namespace Petopia.Business.Models.Common
{
	public class CreateNotificationModel
	{
		public Guid GoalId;
		public string Title = null!;
		public string Content = null!;
		public NotificationType Type;
		public Guid UserId;
	}
}

