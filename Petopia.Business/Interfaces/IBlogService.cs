using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Interfaces
{
  public interface IBlogService
  {
    public Task<PaginationResponseModel<BlogResponseModel>> GetBlogsAsync(PaginationRequestModel<BlogFilterModel> request);
    public Task<BlogDetailResponseModel> GetBlogByIdAsync(Guid id);
    public Task<Guid> CreateBlogAsync(CreateBlogRequestModel request);
    public Task<BlogDetailResponseModel> UpdateBlogAsync(UpdateBlogRequestModel request);
    public Task<PaginationResponseModel<BlogResponseModel>> GetBlogsByUserIdAsync(PaginationRequestModel request);
    public Task<bool> DeleteBlogAsync(Guid id);
    public Task<List<BlogResponseModel>> GetAdvertisementAsync();

  }
}

