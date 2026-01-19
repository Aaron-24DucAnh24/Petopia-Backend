using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    private readonly ISearchEngineService _searchEngineService;

    public PostService(
      IServiceProvider provider,
      ILogger<PostService> logger)
    : base(provider, logger)
    {
      _searchEngineService = provider.GetRequiredService<ISearchEngineService>();
    }

    public async Task<PostResponseModel> CreatePostAsync(CreatePostRequestModel request)
    {
      var post = await UnitOfWork.Posts.CreateAsync(new Post
      {
        Id = Guid.NewGuid(),
        CreatorId = UserContext.Id,
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

      await _searchEngineService.InsertUpdateAsync(Constants.MEILISEARCH_INDEX_POST, result);

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

      await _searchEngineService.DeleteAsync(Constants.MEILISEARCH_INDEX_POST, post.Id.ToString());

      return true;
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

      post.DisplayIndex = CreatePostDisplayIndex(post);
      await UnitOfWork.SaveChangesAsync();

      return post.Like;
    }

    public async Task<PaginationResponseModel<PostResponseModel>> GetPostsAsync(PaginationRequestModel request)
    {
      var posts = UnitOfWork.Posts
        .Include(x => x.Images)
        .Include(x => x.Comments)
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .Where(post => !post.IsDeleted)
        .AsQueryable();
      var result = await PagingAsync<PostResponseModel, Post>(posts, request);
      var userContext = await GetUserContextAsync(UserContext.Id);
      foreach(var post in result.Data)
      {
        post.IsLiked = UnitOfWork.Likes.Any(like => (like.UserId == userContext.Id) && (like.PostId == post.Id));
        post.CommentCount = UnitOfWork.Comments.Count(x => x.PostId == post.Id);
      }

      return result;
    }

    public async Task<bool> ViewPostAsync(Guid postId)
    {
      var post = await UnitOfWork.Posts
        .AsTracking()
        .Where(post => (post.Id == postId) && !post.IsDeleted)
        .FirstOrDefaultAsync()
        ?? throw new DomainException(message: string.Empty);
      post.DisplayIndex = CreatePostDisplayIndex(post);
      UnitOfWork.SaveChange();

      return true;
    }

    #region
    internal static int CreatePostDisplayIndex(Post post)
    {
      // TO DO
      return 0;
    }
    #endregion
  }
}
