#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class Email
  {
    public Guid EmailId { set; get; }
    public string Subject { set; get; }
    public string Body { set; get; }
    public EmailType Type { set; get; }
  }
}