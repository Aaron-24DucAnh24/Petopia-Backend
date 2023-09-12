using Hangfire;
using PetAdoption.BackgroundJobs.Interfaces;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class SampleJobService : BaseJobService, ISampleJobService
  {
    private IBackgroundJobClient JobClient
    { get { return GetRequiredService<IBackgroundJobClient>(); } }

    public SampleJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void LogOnBackground()
    {
      JobClient.Enqueue(() => Console.WriteLine("This is from a background job"));
    }
  }
}