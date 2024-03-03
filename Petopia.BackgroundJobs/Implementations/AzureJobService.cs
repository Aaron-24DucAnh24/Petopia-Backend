using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class AzureJobService : BaseJobService, IAzureJobService
  {
    public AzureJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
  }
}
