using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Post;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Post")]
  public class PostController : ControllerBase
  {
    private readonly IPostService _postService;
    public PostController(
      IPostService postService
    )
    {
      _postService = postService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PostResponseModel>> CreatePost([FromBody] CreatePostRequestModel request)
    {
      return ResponseUtils.OkResult(await _postService.CreatePostAsync(request));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeletePost(Guid id)
    {
      return ResponseUtils.OkResult(await _postService.DeletePostAsync(id));
    }

    [HttpGet("Pet/{petId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<PostResponseModel>>> GetPostsByPetId(Guid petId)
    {
      return ResponseUtils.OkResult(await _postService.GetPostsByPetIdAsync(petId));
    }

    [HttpPut("Like/{postId}")]
    [Authorize]
    public async Task<ActionResult<int>> LikePost(Guid postId)
    {
      return ResponseUtils.OkResult(await _postService.LikePostAsync(postId));
    }
  }
}

