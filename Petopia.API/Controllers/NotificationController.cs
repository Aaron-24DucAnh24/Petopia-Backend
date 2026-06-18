using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Notification;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Notification")]
  public class NotificationController : ControllerBase
  {
    private readonly INotificationService _notificationService;

    public NotificationController(
      INotificationService notificationService
    )
    {
      _notificationService = notificationService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<NotificationResponseModel>>> GetNotifications()
    {
      return ResponseUtils.OkResult(await _notificationService.GetNotificationsAsync());
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<bool>> CheckNotification(Guid id)
    {
      return ResponseUtils.OkResult(await _notificationService.CheckNotificationAsync(id));
    }

    [HttpDelete]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteNotifications()
    {
      return ResponseUtils.OkResult(await _notificationService.DeleteNotificationsAsync());
    }

    [HttpGet("MarkAsSeen")]
    [Authorize]
    public async Task<ActionResult<bool>> MarkAsSeen()
    {
      return ResponseUtils.OkResult(await _notificationService.MarkAsSeenAsync());
    }
  }
}

