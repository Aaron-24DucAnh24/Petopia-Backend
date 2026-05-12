using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business;
using Petopia.Business.Interfaces;
using Petopia.Business.Utils;
using Petopia.Data.Enums;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Storage")]
  public class StorageController : ControllerBase
  {
    private readonly IStorageService _storageService;

    public StorageController(IServiceProvider serviceProvider)
    {
      _storageService = serviceProvider.GetRequiredService<IStorageService>();
    }

    [HttpPost("Upload")]
    [Authorize]
    public async Task<ActionResult<string>> Upload([FromForm] IFormFile image)
    {
      return ResponseUtils.OkResult(await _storageService.UploadFileAsync(Constants.STORAGE_CONTAINER_IMAGE, image));
    }

    [HttpPost("UploadMany")]
    [Authorize]
    public async Task<ActionResult<List<string>>> UploadMany([FromForm] List<IFormFile> images)
    {
      return ResponseUtils.OkResult(await _storageService.UploadFilesAsync(Constants.STORAGE_CONTAINER_IMAGE, images));
    }

    [HttpDelete("Remove")]
    [Authorize]
    public async Task<ActionResult<bool>> Remove([FromForm] string url)
    {
      return ResponseUtils.OkResult(await _storageService.RemoveFileAsync(url));
    }

    [HttpDelete("RemoveMany")]
    [Authorize]
    public async Task<ActionResult<bool>> RemoveMany([FromForm] List<string> urls)
    {
      return ResponseUtils.OkResult(await _storageService.RemoveFilesAsync(urls));
    }
  }
}