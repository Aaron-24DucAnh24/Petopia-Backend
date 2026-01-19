using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
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

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PaginationResponseModel<PostResponseModel>>> GetPosts([FromBody] PaginationRequestModel request)
    {
      return ResponseUtils.OkResult(await _postService.GetPostsAsync(request));
    }

    [HttpPut("Like/{postId}")]
    [Authorize]
    public async Task<ActionResult<int>> LikePost(Guid postId)
    {
      return ResponseUtils.OkResult(await _postService.LikePostAsync(postId));
    }

    [HttpPut("View/{postId}")]
    [Authorize]
    public async Task<ActionResult<bool>> ViewPost(Guid postId)
    {
      return ResponseUtils.OkResult(await _postService.ViewPostAsync(postId));
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
  }
}

