using Microsoft.AspNetCore.SignalR;
using Petopia.Business.Hubs;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
  public class RealTimeService : IRealTimeService
  {
    private readonly IHubContext<RealTimeHub> _hubContext;

    public RealTimeService(IHubContext<RealTimeHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task SendToUserAsync(Guid userId, string eventName, object data)
    {
      await _hubContext.Clients.Group($"user_{userId}").SendAsync(eventName, data);
    }

    public async Task SendToUsersAsync(IEnumerable<Guid> userIds, string eventName, object data)
    {
      var groups = userIds.Select(id => $"user_{id}").ToList();
      foreach (var group in groups)
      {
        await _hubContext.Clients.Group(group).SendAsync(eventName, data);
      }
    }

    public async Task SendToAllAsync(string eventName, object data)
    {
      await _hubContext.Clients.All.SendAsync(eventName, data);
    }
  }
}
