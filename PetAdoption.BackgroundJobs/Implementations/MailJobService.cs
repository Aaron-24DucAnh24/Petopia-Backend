using PetAdoption.BackgroundJobs.Interfaces;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class MailJobService : BaseJobService, IMailJobService
  {
    public MailJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
  }
}