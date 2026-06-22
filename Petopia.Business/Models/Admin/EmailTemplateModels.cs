#nullable disable

namespace Petopia.Business.Models.Admin
{
  public class EmailTemplateResponseModel
  {
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string TypeName { get; set; }
    public string[] Placeholders { get; set; }
  }

}
