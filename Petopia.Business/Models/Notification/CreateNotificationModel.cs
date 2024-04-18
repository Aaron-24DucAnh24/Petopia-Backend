using Petopia.Data.Enums;

namespace Petopia.Business.Models.Notification
{
	public class CreateNotificationModel
	{
		public Guid GoalId;
		public string PetName = null!;
		public string AdopterName = null!;
		public NotificationType Type;
		public Guid UserId;
		public AdoptStatus Status;
	}
}

