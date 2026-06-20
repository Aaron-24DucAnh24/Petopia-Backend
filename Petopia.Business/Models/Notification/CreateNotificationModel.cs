using Petopia.Data.Enums;

namespace Petopia.Business.Models.Notification
{
  public class CreateNotificationModel
  {
    public Guid ReferenceId { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public NotificationType Type { get; set; }
    public Guid UserId { get; set; }
  }
}
