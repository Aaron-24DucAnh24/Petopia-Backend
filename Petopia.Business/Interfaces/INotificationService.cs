using Petopia.Business.Models.Common;
using Petopia.Business.Models.Notification;

namespace Petopia.Business.Interfaces
{
	public interface INotificationService
	{
		public Task<List<NotificationResponseModel>> GetNotificationsAsync();
		public Task<bool> CheckNotificationAsync(Guid id);
		public Task<bool> DeleteNotificationsAsync();
		public Task CreateNoticationAsync(CreateNotificationModel model);
		public Task<bool> MarkAsSeenAsync();
	}
}

