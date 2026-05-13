using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Comment;
using Petopia.Business.Models.Exceptions;
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
      if (request.PostId is not null)
      {
        var post = UnitOfWork.Posts
          .AsTracking()
          .FirstOrDefault(p => p.Id == request.PostId);
        if (post is not null)
        {
          post.LastInteractingDate = DateTimeOffset.Now;
        }
      }

      var comment = await UnitOfWork.Comments.CreateAsync(new Comment
      {
        Id = Guid.NewGuid(),
        Content = request.Content,
        UserId = UserContext.Id,
        PostId = request.PostId,
        BlogId = request.BlogId,
        IsCreatedAt = DateTimeOffset.Now,
      });
      await UnitOfWork.SaveChangesAsync();

      var userContext = await GetUserContextAsync(UserContext.Id);
      var result = new CommentResponseModel
      {
        Id = comment.Id,
        UserId = comment.UserId,
        UserImage = userContext.Image,
        UserName = userContext.Name,
        Content = comment.Content,
        IsCreatedAt = comment.IsCreatedAt,
      };

      return result;
    }

    public async Task<CommentResponseModel> UpdateCommentAsync(UpdateCommentRequestModel request)
    {
      var comment = await UnitOfWork.Comments
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id)
        ?? throw new DomainException("Comment not found.") { ErrorCode = DomainErrorCode.NOT_FOUND_COMMENT };

      if (comment.UserId != UserContext.Id)
      {
        throw new DomainException("You are not authorized to update this comment.") { ErrorCode = DomainErrorCode.UNAUTHORIZED_COMMENT };
      }

      comment.Content = request.Content;
      UnitOfWork.Comments.Update(comment);
      await UnitOfWork.SaveChangesAsync();

      var userContext = await GetUserContextAsync(UserContext.Id);
      return new CommentResponseModel
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
      var comment = await UnitOfWork.Comments.FirstOrDefaultAsync(x => x.Id == id);
      if ((comment is not null)
        && (comment.UserId == UserContext.Id))
      {
        UnitOfWork.Comments.Delete(comment);
        await UnitOfWork.SaveChangesAsync();
        return true;
      }

      return false;
    }

    public async Task<List<CommentResponseModel>> GetCommentsByBlogIdAsync(Guid blogId)
    {
      var comments = await UnitOfWork.Comments
        .Where(x => x.PostId == blogId)
        .ToListAsync();
      var result = Mapper.Map<List<CommentResponseModel>>(comments);
      foreach (var comment in result)
      {
        var userContext = await GetUserContextAsync(comment.UserId);
        comment.UserName = userContext.Name;
        comment.UserImage = userContext.Image;
      }

      return result;
    }

    public async Task<List<CommentResponseModel>> GetCommentsByPostIdAsync(Guid postId)
    {
      var comments = await UnitOfWork.Comments
        .Where(x => x.PostId == postId)
        .OrderByDescending(x => x.IsCreatedAt)
        .ToListAsync();
      var result = Mapper.Map<List<CommentResponseModel>>(comments);
      foreach (var comment in result)
      {
        var userContext = await GetUserContextAsync(comment.UserId);
        comment.UserName = userContext.Name;
        comment.UserImage = userContext.Image;
      }

      return result;
    }
  }
}

