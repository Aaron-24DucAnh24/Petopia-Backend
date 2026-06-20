namespace Petopia.Business.Interfaces
{
  public interface IRealTimeService
  {
    Task SendToUserAsync(Guid userId, string eventName, object data);
    Task SendToUsersAsync(IEnumerable<Guid> userIds, string eventName, object data);
    Task SendToAllAsync(string eventName, object data);
  }
}
