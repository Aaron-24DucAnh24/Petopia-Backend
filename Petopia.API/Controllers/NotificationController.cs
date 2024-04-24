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
    private readonly INotificationService _noticationService;

    public NotificationController(
      INotificationService notificationService
    )
    {
      _noticationService = notificationService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<NotificationResponseModel>>> GetNotications()
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

    [HttpGet("MarkAsSeen")]
    [Authorize]
    public async Task<ActionResult<bool>> MarkAsSeen()
    {
      return ResponseUtils.OkResult(await _noticationService.MarkAsSeenAsync());
    }
  }
}

