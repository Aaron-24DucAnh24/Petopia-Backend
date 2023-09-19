using Hangfire;
using Microsoft.AspNetCore.Http;
using PetAdoption.BackgroundJobs.Interfaces;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class StorageJobService : BaseJobService, IStorageJobService
  {
    private IStorageService _storageService
    { get { return GetRequiredService<IStorageService>(); } }
    public StorageJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void RemoveFileAsync(string url)
    {
      JobClient.Enqueue(() => _storageService.RemoveFileAsync(url));
    }

    public void UploadFileAsync(IFormFile file, string url)
    {
      JobClient.Enqueue(() => _storageService.UploadFileAsync(file, url));
    }
  }
}