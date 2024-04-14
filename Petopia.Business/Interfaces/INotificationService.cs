using Petopia.Business.Models.Common;
using Petopia.Data.Entities;

namespace Petopia.Business.Interfaces
{
	public interface INotificationService
	{
		public Task<List<Notification>> GetNotificationsAsync();
		public Task<bool> CheckNotificationAsync(Guid id);
		public Task<bool> DeleteNotificationsAsync();
		public Task CreateNoticationAsync(CreateNotificationModel model);
	}
}

