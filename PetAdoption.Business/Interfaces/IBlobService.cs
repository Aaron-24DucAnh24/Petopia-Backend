using Microsoft.AspNetCore.Http;

namespace PetAdoption.Business.Interfaces
{
  public interface IBlobService
  {
    public Task<string> UpLoadImageAsync(IFormFile file, string name);
    public Task<bool> RemoveImageAsync(string blogName);
  }
}