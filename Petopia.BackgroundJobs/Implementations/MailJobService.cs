using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class MailJobService : BaseJobService, IMailJobService
  {
    public MailJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
  }
}