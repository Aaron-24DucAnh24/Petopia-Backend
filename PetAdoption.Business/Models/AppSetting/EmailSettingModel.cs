namespace PetAdoption.Business.Models
{
  public class EmailSettingModel
  {
    public string EmailClient { get; set; } = null!;
    public string SmtpClient { get; set; } = null!;
    public string Password { get; set; } = null!;
  }
}