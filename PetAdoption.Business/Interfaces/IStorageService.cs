using Microsoft.AspNetCore.Http;

namespace PetAdoption.Business.Interfaces
{
  public interface IStorageService
  {
    public Task UploadFileAsync(IFormFile file, string url);
    public Task RemoveFileAsync(string url);
  }
}