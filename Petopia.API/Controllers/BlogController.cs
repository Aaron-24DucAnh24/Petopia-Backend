using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Common;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Blog")]
  public class BlogController : ControllerBase
  {
    private readonly IBlogService _blogService;

    public BlogController(
      IBlogService blogService
    )
    {
      _blogService = blogService;
    }

    [HttpPost]
    [OrganizationAuthorize]
    public async Task<ActionResult<Guid>> CreateBlog([FromBody] CreateBlogRequestModel request)
    {
      return ResponseUtils.OkResult(await _blogService.CreateBlogAsync(request));
    }

    [HttpPut]
    [OrganizationAuthorize]
    public async Task<ActionResult<BlogDetailResponseModel>> UpdateBlog([FromBody] UpdateBlogRequestModel request)
    {
      return ResponseUtils.OkResult(await _blogService.UpdateBlogAsync(request));
    }

    [HttpPost("Get")]
    public async Task<ActionResult<PaginationResponseModel<BlogResponseModel>>> GetBlogs([FromBody] PaginationRequestModel request)
    {
      return ResponseUtils.OkResult(await _blogService.GetBlogsAsync(request));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BlogDetailResponseModel>> GetBlogs(Guid id)
    {
      return ResponseUtils.OkResult(await _blogService.GetBlogByIdAsync(id));
    }
  }
}