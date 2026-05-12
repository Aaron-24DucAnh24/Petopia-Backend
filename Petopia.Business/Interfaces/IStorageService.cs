using Microsoft.AspNetCore.Http;

namespace Petopia.Business.Interfaces
{
  public interface IStorageService
  {
    public Task<string> UploadFileAsync(string container, IFormFile file);
    public Task<bool> RemoveFileAsync(string fileUrl);
    public Task<List<string>> UploadFilesAsync(string container, List<IFormFile> files);
    public Task<bool> RemoveFilesAsync(List<string> fileUrls);
  }
}