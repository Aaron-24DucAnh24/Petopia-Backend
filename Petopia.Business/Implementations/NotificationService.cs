using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
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
			Notification note = await UnitOfWork.Notifications
				.AsTracking()
				.FirstAsync(x => x.Id == id);
			note.IsChecked = true;
			UnitOfWork.Notifications.Update(note);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task CreateNoticationAsync(CreateNotificationModel model)
		{
			await UnitOfWork.Notifications.CreateAsync(new Notification()
			{
				Id = Guid.NewGuid(),
				Content = model.Content,
				Title = model.Title,
				IsChecked = false,
				UserId = model.UserId,
				GoalId = model.GoalId,
				Type = model.Type,
			});
			await UnitOfWork.SaveChangesAsync();
			return;
		}

		public async Task<bool> DeleteNotificationsAsync()
		{
			List<Notification> notes = await UnitOfWork.Notifications
				.Where(x => x.UserId == UserContext.Id)
				.ToListAsync();
			foreach(var note in notes)
			{
				UnitOfWork.Notifications.Delete(note);
			}
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<List<Notification>> GetNotificationsAsync()
		{
			return await UnitOfWork.Notifications
			.Where(x => x.UserId == UserContext.Id)
			.ToListAsync();
		}
	}
}

