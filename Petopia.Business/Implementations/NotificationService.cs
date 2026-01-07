using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Notification;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class NotificationService : BaseService, INotificationService
  {
    public NotificationService(
      IServiceProvider provider,
      ILogger<NotificationService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<bool> CheckNotificationAsync(Guid id)
    {
      Notification? note = await UnitOfWork.Notifications
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserContext.Id);

      if (note == null)
      {
        return false;
      }

      note.IsChecked = true;
      UnitOfWork.Notifications.Update(note);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> DeleteNotificationsAsync()
    {
      List<Notification> notes = await UnitOfWork.Notifications
        .Where(x => x.UserId == UserContext.Id)
        .ToListAsync();
      foreach (var note in notes)
      {
        UnitOfWork.Notifications.Delete(note);
      }
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<List<NotificationResponseModel>> GetNotificationsAsync()
    {
      List<Notification> note = await UnitOfWork.Notifications
        .Where(x => x.UserId == UserContext.Id)
        .ToListAsync();

      return Mapper.Map<List<NotificationResponseModel>>(note.OrderByDescending(x => x.IsCreatedAt).ToList());
    }

    public async Task<bool> MarkAsSeenAsync()
    {
      List<Notification> notes = await UnitOfWork.Notifications
        .AsTracking()
        .Where(x => x.UserId == UserContext.Id)
        .ToListAsync();
      foreach (var note in notes)
      {
        note.IsChecked = true;
        UnitOfWork.Notifications.Update(note);
      }
      await UnitOfWork.SaveChangesAsync();
      return true;
    }
  }
}

