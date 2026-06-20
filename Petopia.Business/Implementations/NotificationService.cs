using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Notification;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class NotificationService : BaseService, INotificationService
  {
    private readonly IRealTimeService _realTimeService;

    public NotificationService(
      IServiceProvider provider,
      ILogger<NotificationService> logger
    ) : base(provider, logger)
    {
      _realTimeService = provider.GetRequiredService<IRealTimeService>();
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

    public async Task<bool> DeleteNotificationAsync(Guid id)
    {
      Notification? note = await UnitOfWork.Notifications
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserContext.Id);

      if (note == null)
      {
        return false;
      }

      UnitOfWork.Notifications.Delete(note);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<List<NotificationResponseModel>> GetNotificationsAsync()
    {
      List<Notification> note = await UnitOfWork.Notifications
        .Where(x => x.UserId == UserContext.Id)
        .OrderByDescending(x => x.IsCreatedAt)
        .ToListAsync();

      return Mapper.Map<List<NotificationResponseModel>>(note);
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

    public async Task CreateAsync(CreateNotificationModel model)
    {
      var notification = new Notification
      {
        Id = Guid.NewGuid(),
        ReferenceId = model.ReferenceId,
        ReferenceType = model.ReferenceType,
        Title = model.Title,
        Content = model.Content,
        Type = model.Type,
        UserId = model.UserId,
        IsChecked = false,
        IsCreatedAt = DateTimeOffset.UtcNow,
      };

      await UnitOfWork.Notifications.CreateAsync(notification);
      await UnitOfWork.SaveChangesAsync();

      var response = Mapper.Map<NotificationResponseModel>(notification);
      await _realTimeService.SendToUserAsync(model.UserId, "ReceiveNotification", response);
    }

    public async Task CreateForAdminsAsync(CreateNotificationModel model)
    {
      var admins = await UnitOfWork.Users
        .Where(x => x.Role == UserRole.SystemAdmin)
        .ToListAsync();

      var notifications = new List<Notification>();

      foreach (var admin in admins)
      {
        var notification = new Notification
        {
          Id = Guid.NewGuid(),
          ReferenceId = model.ReferenceId,
          ReferenceType = model.ReferenceType,
          Title = model.Title,
          Content = model.Content,
          Type = model.Type,
          UserId = admin.Id,
          IsChecked = false,
          IsCreatedAt = DateTimeOffset.UtcNow,
        };

        await UnitOfWork.Notifications.CreateAsync(notification);
        notifications.Add(notification);
      }

      await UnitOfWork.SaveChangesAsync();

      foreach (var notification in notifications)
      {
        var response = Mapper.Map<NotificationResponseModel>(notification);
        await _realTimeService.SendToUserAsync(notification.UserId, "ReceiveNotification", response);
      }
    }
  }
}
