using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Models;
using PetAdoption.Business.Utils;

namespace PetAdoption.Business.Implementations
{
  public class StorageService : IStorageService
  {
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public StorageService(IServiceProvider provider)
    {
      _configuration = provider.GetRequiredService<IConfiguration>();
      _blobServiceClient = new BlobServiceClient
      (
        _configuration.GetSection(AppSettingKey.STORAGE).Get<StorageSettingModel>()?.ConnectionString
      );
    }

    public async Task RemoveFileAsync(string url)
    {
      var blobName = StorageUtil.GetBlobFromUrl(url);
      var containerClient = _blobServiceClient.GetBlobContainerClient(StorageUtil.GetContainerFromUrl(url));
      if (containerClient.GetBlobClient(blobName).Exists())
      {
        await containerClient.DeleteBlobAsync(blobName);
      }
    }

    public async Task UploadFileAsync(IFormFile file, string url)
    {
      var containerClient = _blobServiceClient.GetBlobContainerClient(StorageUtil.GetContainerFromUrl(url));
      var blobClient = containerClient.GetBlobClient(StorageUtil.GetBlobFromUrl(url));
      var stream = file.OpenReadStream();

      await blobClient.UploadAsync
      (
        stream,
        httpHeaders: new BlobHttpHeaders { ContentType = file.ContentType },
        conditions: null
      );
    }
  }
}