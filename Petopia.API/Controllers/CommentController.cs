using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Comment;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Comment")]
  public class CommentController : ControllerBase
  {
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
      _commentService = commentService;
    }

    [HttpGet("blog/{blogId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CommentResponseModel>>> GetCommentsByBlogId(Guid blogId)
    {
      return ResponseUtils.OkResult(await _commentService.GetCommentsByBlogIdAsync(blogId));
    }

    [HttpGet("post/{postId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CommentResponseModel>>> GetCommentsByPostId(Guid postId)
    {
      return ResponseUtils.OkResult(await _commentService.GetCommentsByPostIdAsync(postId));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentResponseModel>> CreateComment([FromBody] CreateCommentRequestModel request)
    {
      return ResponseUtils.OkResult(await _commentService.CreateCommentAsync(request));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteComment(Guid id)
    {
      return ResponseUtils.OkResult(await _commentService.DeleteCommentAsync(id));
    }
  }
}

