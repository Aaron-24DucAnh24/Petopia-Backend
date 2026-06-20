using Petopia.Business.Models.Notification;

namespace Petopia.Business.Interfaces
{
  public interface INotificationService
  {
    Task<List<NotificationResponseModel>> GetNotificationsAsync();
    Task<bool> CheckNotificationAsync(Guid id);
    Task<bool> DeleteNotificationAsync(Guid id);
    Task<bool> MarkAsSeenAsync();
    Task CreateAsync(CreateNotificationModel model);
    Task CreateForAdminsAsync(CreateNotificationModel model);
  }
}
