using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Notification;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

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
			string content, title;
			switch (model.Status)
			{
				case AdoptStatus.Pending:
					title = string.Join(" ", "Có người muốn nhận nuôi", model.PetName, "nè!");
					content = string.Join(" ", "Đơn nhận nuôi", model.PetName, "từ", model.AdopterName);
					break;

				case AdoptStatus.Cancel:
					title = string.Join(" ", "Một yêu cầu nhận nuôi", model.PetName, "đã bị huỷ");
					content = string.Join(" ", "Đơn nhận nuôi", model.PetName, "từ", model.AdopterName, "đã bị huỷ");
					break;

				case AdoptStatus.Accepted:
					title = "Yêu cầu nhận nuôi của bạn được chấp nhận!";
					content = string.Join(" ", "Đơn nhận nuôi", model.PetName, "đã được chấp nhận");
					break;

				case AdoptStatus.Rejected:
					title = "Yêu cầu nhận nuôi của bạn bị từ chối!";
					content = string.Join(" ", "Đơn nhận nuôi", model.PetName, "đã bị từ chối");
					break;

				default:
					title = "Quá trình nhận nuôi hoàn tất!";
					content = string.Join(" ", model.AdopterName, "đã nhận nuôi thành công", model.PetName);
					break;
			}

			await UnitOfWork.Notifications.CreateAsync(new Notification()
			{
				Id = Guid.NewGuid(),
				Content = content,
				Title = title,
				IsChecked = false,
				UserId = model.UserId,
				GoalId = model.GoalId,
				Type = model.Type,
				IsCreatedAt = DateTimeOffset.Now,
			});
			await UnitOfWork.SaveChangesAsync();
			return;
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
			note.OrderByDescending(x => x.IsCreatedAt);
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
	}
}

