using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Petopia.Business.Hubs
{
  [Authorize]
  public class RealTimeHub : Hub
  {
    public override async Task OnConnectedAsync()
    {
      var userId = Context.User?.FindFirst(Constants.CLAIM_TYPE_ID)?.Value;
      if (userId != null)
      {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{Constants.SIGNALR_USER_GROUP_PREFIX}{userId}");
      }
      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
      await base.OnDisconnectedAsync(exception);
    }
  }
}
