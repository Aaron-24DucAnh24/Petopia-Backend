using Hangfire;
using Microsoft.Extensions.DependencyInjection;
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
      BackgroundJob.Enqueue(() => ServiceProvider.GetRequiredService<IEmailService>().SendMailAsync(data));
    }
  }
}