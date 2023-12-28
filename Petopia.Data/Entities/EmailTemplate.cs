#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class EmailTemplate
  {
    public Guid Id { set; get; }
    public string Subject { set; get; }
    public string Body { set; get; }
    public EmailType Type { set; get; }
  }
}