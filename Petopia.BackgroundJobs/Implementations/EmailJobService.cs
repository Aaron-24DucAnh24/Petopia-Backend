using Hangfire;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Email;

namespace Petopia.BackgroundJobs.Implementations
{
  public class EmailJobService : BaseJobService, IEmailJobService
  {
    public EmailJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void SendMail(MailDataModel data)
    {
      JobClient.Enqueue(() => GetRequiredService<IEmailService>().SendMailAsync(data));
    }
  }
}