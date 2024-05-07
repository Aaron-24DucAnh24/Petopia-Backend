using System.Configuration;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Email;
using Petopia.Business.Models.Setting;

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

    public void SendUpgradeMails()
    {
      RecurringJobSettingModel settings = Configuration
        .GetSection(AppSettingKey.RECURRING_JOB)
        .Get<RecurringJobSettingModel>()
        ?? throw new ConfigurationErrorsException();

      RecurringJob.AddOrUpdate(
        settings.OrgJobId,
        () => ServiceProvider.GetRequiredService<IEmailService>().SendUpgradeMailsAsync(),
        settings.Cron
      );

      RecurringJob.AddOrUpdate(
        settings.AdminJobId,
        () => ServiceProvider.GetRequiredService<IEmailService>().SendAdminMailsAsync(),
        settings.Cron
      );
    }
  }
}