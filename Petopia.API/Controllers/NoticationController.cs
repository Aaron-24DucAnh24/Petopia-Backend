using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Utils;
using Petopia.Data.Entities;

namespace Petopia.API.Controllers
{
	[ApiController]
	[Route("api/Notification")]
	public class NoticationController : ControllerBase
	{
		private readonly INotificationService _noticationService;

		public NoticationController(
			INotificationService notificationService
		)
		{
			_noticationService = notificationService;
		}

		[HttpGet]
		[Authorize]
		public async Task<ActionResult<List<Notification>>> GetNotications()
		{
			return ResponseUtils.OkResult(await _noticationService.GetNotificationsAsync());
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<bool>> CheckNotification(Guid id)
		{
			return ResponseUtils.OkResult(await _noticationService.CheckNotificationAsync(id));
		}

		[HttpDelete]
		[Authorize]
		public async Task<ActionResult<bool>> DeleteNotifications()
		{
			return ResponseUtils.OkResult(await _noticationService.DeleteNotificationsAsync());
		}
	}
}

