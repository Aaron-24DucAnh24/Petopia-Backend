namespace Petopia.Business.Models.Setting
{
  public class EmailSettingModel
  {
    public string EmailClient { get; set; } = null!;
    public string SmtpClient { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Port { get; set; }
  }
}