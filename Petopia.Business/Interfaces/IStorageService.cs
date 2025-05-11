using Microsoft.AspNetCore.Http;

namespace Petopia.Business.Interfaces
{
  public interface IStorageService
  {
    public Task<string> UploadFileAsync(string container, IFormFile file);
    public Task<bool> RemoveFileAsync(string fileUrl);
  }
}