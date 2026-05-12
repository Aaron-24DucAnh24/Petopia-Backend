using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Post;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class PostService : BaseService, IPostService
  {
    public PostService(
      IServiceProvider provider,
      ILogger<PostService> logger)
    : base(provider, logger)
    {
    }

    public async Task<PaginationResponseModel<PostResponseModel>> GetPostsAsync(PaginationRequestModel<GetPostsFilterModel> request)
    {
      var posts = UnitOfWork.Posts
        .Include(x => x.Images)
        .Include(x => x.Comments)
        .Include(x => x.User)
        .ThenInclude(x => x.UserIndividualAttributes)
        .Where(x => !x.IsDeleted)
        .OrderByDescending(x => x.LastInteractingDate)
        .AsQueryable();
      if (request.Filter.UserId != null)
      {
        posts = posts.Where(x => x.UserId == request.Filter.UserId);
      }

      var result = await PagingAsync<PostResponseModel, Post>(posts, request);
      var userContext = UserContext.Email.IsNullOrEmpty()
        ? null
        : await GetUserContextAsync(UserContext.Id);
      foreach (var post in result.Data)
      {
        post.CommentCount = UnitOfWork.Comments.Count(x => x.PostId == post.Id);
        if (userContext is not null)
        {
          post.IsLiked = UnitOfWork.Likes.Any(like => (like.UserId == userContext.Id) && (like.PostId == post.Id));
        }
      }

      return result;
    }

    public async Task<PostResponseModel> GetPostAsync(Guid postId)
    {
      var post = await UnitOfWork.Posts
        .Include(x => x.Images)
        .Include(x => x.Comments)
        .Include(x => x.User)
        .ThenInclude(x => x.UserIndividualAttributes)
        .OrderByDescending(x => x.LastInteractingDate)
        .FirstOrDefaultAsync(x => (x.Id == postId) && !x.IsDeleted)
        ?? throw new DomainException("No post found.");
      var result = Mapper.Map<PostResponseModel>(post);
      result.CommentCount = UnitOfWork.Comments.Count(x => x.PostId == post.Id);
      if (!UserContext.Email.IsNullOrEmpty())
      {
        var userContext = await GetUserContextAsync(UserContext.Id);
        result.IsLiked = UnitOfWork.Likes.Any(like => (like.UserId == userContext.Id) && (like.PostId == post.Id));
      }

      return result;
    }

    public async Task<int> LikePostAsync(Guid postId)
    {
      var post = await UnitOfWork.Posts
        .AsTracking()
        .FirstAsync(x => x.Id == postId);
      var like = await UnitOfWork.Likes
        .Where(x => x.PostId == postId && x.UserId == UserContext.Id)
        .FirstOrDefaultAsync();
      if (like is not null)
      {
        post.Like -= 1;
        UnitOfWork.Likes.Delete(like);
      }
      else
      {
        post.Like += 1;
        await UnitOfWork.Likes.CreateAsync(new Like
        {
          Id = Guid.NewGuid(),
          PostId = postId,
          UserId = UserContext.Id,
        });
      }

      post.LastInteractingDate = DateTimeOffset.Now;
      await UnitOfWork.SaveChangesAsync();

      return post.Like;
    }

    public async Task<bool> ViewPostAsync(Guid postId)
    {
      var post = await UnitOfWork.Posts
        .AsTracking()
        .Where(post => (post.Id == postId) && !post.IsDeleted)
        .FirstOrDefaultAsync()
        ?? throw new DomainException(message: string.Empty);
      post.LastInteractingDate = DateTimeOffset.Now;
      UnitOfWork.SaveChange();

      return true;
    }

    public async Task<PostResponseModel> CreatePostAsync(CreatePostRequestModel request)
    {
      var post = await UnitOfWork.Posts.CreateAsync(new Post
      {
        Id = Guid.NewGuid(),
        UserId = UserContext.Id,
        Content = request.Content,
        IsCreatedAt = DateTimeOffset.Now,
      });

      foreach (var image in request.Images)
      {
        await UnitOfWork.Medias.CreateAsync(new Media
        {
          Id = Guid.NewGuid(),
          PostId = post.Id,
          Url = image,
          Type = MediaType.Image,
        });
      }

      await UnitOfWork.SaveChangesAsync();

      var result = Mapper.Map<PostResponseModel>(post);
      var userContext = await GetUserContextAsync(UserContext.Id);
      result.Images = request.Images;
      result.UserName = userContext.Name;
      result.UserImage = userContext.Image;

      return result;
    }

    public async Task<bool> DeletePostAsync(Guid id)
    {
      var post = await UnitOfWork.Posts.FirstOrDefaultAsync(x => x.Id == id);
      if (post is null) return false;

      await UnitOfWork.Medias.DeleteAllAsync(x => x.PostId == id);
      await UnitOfWork.Comments.DeleteAllAsync(x => x.PostId == id);
      UnitOfWork.Posts.Delete(post);
      await UnitOfWork.SaveChangesAsync();

      return true;
    }
  }
}
