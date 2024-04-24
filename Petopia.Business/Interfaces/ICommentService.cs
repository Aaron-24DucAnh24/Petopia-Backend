using Petopia.Business.Models.Comment;

namespace Petopia.Business.Interfaces
{
  public interface ICommentService
  {
    public Task<List<CommentResponseModel>> GetCommentsByBlogIdAsync(Guid blogId);
    public Task<List<CommentResponseModel>> GetCommentsByPostIdAsync(Guid blogId);
    public Task<CommentResponseModel> CreateCommentAsync(CreateCommentRequestModel request);
    public Task<bool> DeleteCommentAsync(Guid id);
  }
}