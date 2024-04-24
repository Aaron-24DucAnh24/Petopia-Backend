using Petopia.Business.Models.Email;

namespace Petopia.BackgroundJobs.Interfaces
{
  public interface IEmailJobService
  {
    public void SendMail(MailDataModel data);
    public void SendUpgradeMails();
  }
}