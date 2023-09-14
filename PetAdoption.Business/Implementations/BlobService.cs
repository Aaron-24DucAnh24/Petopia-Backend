using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class BlobService : IBlobService
  {
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public BlobService(IServiceProvider provider)
    {
      _configuration = provider.GetService<IConfiguration>() ?? throw new Exception("Service not found");
      _blobServiceClient = new BlobServiceClient(
				_configuration.GetConnectionString(AppSettingKey.BLOG_STORAGE_CONNECTION_STRING));
    }

    public async Task<bool> RemoveImageAsync(string blogName)
    {
			BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName.IMAGE);

			try
			{
				await containerClient.DeleteBlobAsync(blogName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
    }

    public async Task<string> UpLoadImageAsync(IFormFile file, string name)
    {
			BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName.IMAGE);
			BlobClient blobClient = containerClient.GetBlobClient(name);
			Stream stream = file.OpenReadStream();

			await blobClient.UploadAsync(
				stream,
				httpHeaders: new BlobHttpHeaders { ContentType = file.ContentType },
				conditions: null);

			return blobClient.Uri.ToString();
    }
  }
}