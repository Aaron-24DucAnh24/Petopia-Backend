using System.Configuration;
using Microsoft.Extensions.Configuration;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Models;

namespace PetAdoption.Business.Utils
{
  public static class StorageUtil
  {
    public static string GetContainerFromUrl(string url)
    {
      return url.Split("/", StringSplitOptions.RemoveEmptyEntries).ToArray()[3];
    }

    public static string GetBlobFromUrl(string url)
    {
      return url.Split("/", StringSplitOptions.RemoveEmptyEntries).ToArray()[4];
    }

    public static string GetUrlFromFileName(string fileName, IConfiguration configuration)
    {
      var uploadContentSettings = configuration
        .GetSection(AppSettingKey.UPLOAD_CONTENT)
        .Get<UploadContentSettingModel>();
      var storageSettings = configuration
        .GetSection(AppSettingKey.STORAGE)
        .Get<StorageSettingModel>();
      if (uploadContentSettings == null || storageSettings == null)
      {
        throw new ConfigurationErrorsException();
      }
      var fileExtension = Path.GetExtension(fileName).Substring(1);
      fileName = Guid.NewGuid().ToString();
      if (uploadContentSettings.Image.Contains(fileExtension))
      {
        return storageSettings.MountedPath
          + "/" + BlobContainerName.IMAGE
          + "/" + fileName;
      }
      if (uploadContentSettings.Video.Contains(fileExtension))
      {
        return storageSettings.MountedPath
          + "/" + BlobContainerName.VIDEO
          + "/" + fileName;
      }
      return string.Empty;
    }
  }
}