using Petopia.Business.Models.Common;
using Petopia.Business.Models.Post;

namespace Petopia.Business.Interfaces
{
  public interface IPostService
  {
    public Task<PaginationResponseModel<PostResponseModel>> GetPostsAsync(PaginationRequestModel request);
    public Task<int> LikePostAsync(Guid postId);
    public Task<bool> ViewPostAsync(Guid postId);
    public Task<PostResponseModel> CreatePostAsync(CreatePostRequestModel request);
    public Task<bool> DeletePostAsync(Guid postId);
  }
}

