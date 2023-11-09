namespace Petopia.Business.Models.Email
{
  public class MailDataModel
  {
    public string From { get; set; } = null!;
    public List<string> To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public bool IsBodyHtml { get; set; }
  }
}