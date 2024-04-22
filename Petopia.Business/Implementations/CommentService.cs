using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Comment;
using Petopia.Business.Models.User;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
	public class CommentService : BaseService, ICommentService
	{
		public CommentService(
			IServiceProvider provider,
			ILogger<CommentService> logger
		) : base(provider, logger)
		{
		}

		public async Task<CommentResponseModel> CreateCommentAsync(CreateCommentRequestModel request)
		{
			Comment comment = await UnitOfWork.Comments.CreateAsync(new Comment()
				{
					Id = Guid.NewGuid(),
					Content = request.Content,
					UserId = UserContext.Id,
					PostId = request.PostId,
					BlogId = request.BlogId,
					IsCreatedAt = DateTimeOffset.Now,
				});
			UserContextModel userContext = await GetUserContextAsync(UserContext.Id);
			return new CommentResponseModel()
			{
				Id = comment.Id,
				UserId = comment.UserId,
				UserImage = userContext.Image,
				UserName = userContext.Name,
				Content = comment.Content,
				IsCreatedAt = comment.IsCreatedAt,
			};
		}

		public async Task<bool> DeleteCommentAsync(Guid id)
		{
			await UnitOfWork.Comments.DeleteAllAsync(x => x.Id == id);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByBlogIdAsync(Guid blogId)
		{
			List<Comment> comments = await UnitOfWork.Comments
				.Where(x => x.PostId == blogId)
				.ToListAsync();

			var result = Mapper.Map<List<CommentResponseModel>>(comments);
			foreach (var comment in result)
			{
				UserContextModel userContext = await GetUserContextAsync(comment.UserId);
				comment.UserName = userContext.Name;
				comment.UserImage = userContext.Image;
			}
			return result;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByPostIdAsync(Guid postId)
		{
			List<Comment> comments = await UnitOfWork.Comments
				.Where(x => x.PostId == postId)
				.ToListAsync();

			var result = Mapper.Map<List<CommentResponseModel>>(comments);
			foreach(var comment in result)
			{
				UserContextModel userContext = await GetUserContextAsync(comment.UserId);
				comment.UserName = userContext.Name;
				comment.UserImage = userContext.Image;
			}
			return result;
		}
	}
}

