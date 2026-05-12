using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Setting;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class StorageService : BaseService, IStorageService
  {
    private readonly IMinioClient _minioClient;
    private readonly MinioSettingModel _minioSettings;

    public StorageService(
      IServiceProvider provider,
      ILogger<StorageService> logger,
      MinioSettingModel minioSetting
    ) : base(provider, logger)
    {
      _minioSettings = minioSetting;
      _minioClient = new MinioClient()
          .WithEndpoint(minioSetting.Endpoint)
          .WithCredentials(minioSetting.User, minioSetting.Password)
          .Build();
    }

    public async Task<bool> RemoveFileAsync(string fileUrl)
    {
      try
      {
        var uri = new Uri(fileUrl);
        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length < 2) return false;

        var container = segments[0];
        var fileName = string.Join('/', segments.Skip(1));
        var args = new RemoveObjectArgs()
            .WithBucket(container)
            .WithObject(fileName);

        await _minioClient.RemoveObjectAsync(args);

        return true;
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return false;
      }
    }

    public async Task<string> UploadFileAsync(string container, IFormFile file)
    {
      var fileName = $"{Guid.NewGuid()}_{file.FileName}";
      var result = string.Empty;
      try
      {
        using var stream = file.OpenReadStream();
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(container)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putObjectArgs);
        result = $"http://{_minioSettings.Endpoint}/{container}/{fileName}";
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
      }
      return result;
    }

    public async Task<List<string>> UploadFilesAsync(string container, List<IFormFile> files)
    {
      var results = new List<string>();

      if ((files == null) || (files.Count == 0)) return results;

      foreach (var file in files)
      {
        if ((file == null) || (file.Length == 0)) continue;

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        try
        {
          using var stream = file.OpenReadStream();
          var putObjectArgs = new PutObjectArgs()
              .WithBucket(container)
              .WithObject(fileName)
              .WithStreamData(stream)
              .WithObjectSize(file.Length)
              .WithContentType(file.ContentType);

          await _minioClient.PutObjectAsync(putObjectArgs);

          var fileUrl = $"http://{_minioSettings.Endpoint}/{container}/{fileName}";
          results.Add(fileUrl);
        }
        catch (Exception ex)
        {
          Logger.LogError(ex, $"Upload failed for file {file.FileName}");
        }
      }

      return results;
    }

    public async Task<bool> RemoveFilesAsync(List<string> fileUrls)
    {
      if ((fileUrls == null) || (fileUrls.Count == 0)) return false;


      var success = true;

      foreach (var fileUrl in fileUrls)
      {
        try
        {
          var uri = new Uri(fileUrl);
          var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

          if (segments.Length < 2) continue;

          var container = segments[0];
          var fileName = string.Join('/', segments.Skip(1));

          var args = new RemoveObjectArgs()
              .WithBucket(container)
              .WithObject(fileName);

          await _minioClient.RemoveObjectAsync(args);
        }
        catch (Exception ex)
        {
          success = false;
          Logger.LogError(ex, $"Remove failed for {fileUrl}");
        }
      }

      return success;
    }
  }
}