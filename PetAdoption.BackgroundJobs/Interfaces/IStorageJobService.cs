using Microsoft.AspNetCore.Http;

namespace PetAdoption.BackgroundJobs.Interfaces
{
  public interface IStorageJobService
  {
    void RemoveFileAsync(string url);
    void UploadFileAsync(IFormFile file, string url);
  }
}