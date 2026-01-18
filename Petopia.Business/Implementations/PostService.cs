using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
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

      await UnitOfWork.SaveChangesAsync();

      return post.Like;
    }
  }
}
