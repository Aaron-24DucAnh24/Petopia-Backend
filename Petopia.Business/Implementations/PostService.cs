using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Post;
using Petopia.Business.Models.User;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class PostService : BaseService, IPostService
  {
    public PostService(IServiceProvider provider, ILogger<PostService> logger) : base(provider, logger)
    {
    }

    public async Task<PostResponseModel> CreatePostAsync(CreatePostRequestModel request)
    {
      Post post = await UnitOfWork.Posts.CreateAsync(new Post()
      {
        Id = Guid.NewGuid(),
        CreatorId = UserContext.Id,
        PetId = request.PetId,
        Content = request.Content,
        IsCreatedAt = DateTimeOffset.Now,
      });

      foreach (var image in request.Images)
      {
        await UnitOfWork.Medias.CreateAsync(new Media()
        {
          Id = Guid.NewGuid(),
          PostId = post.Id,
          Url = image,
          Type = MediaType.Image,
        });
      }

      await UnitOfWork.SaveChangesAsync();
      var result = Mapper.Map<PostResponseModel>(post);
      UserContextModel userContext = await GetUserContextAsync(UserContext.Id);
      result.Images = request.Images;
      result.UserName = userContext.Name;
      result.UserImage = userContext.Image;
      return result;
    }

    public async Task<bool> DeletePostAsync(Guid id)
    {
      await UnitOfWork.Medias.DeleteAllAsync(x => x.PostId == id);
      await UnitOfWork.Comments.DeleteAllAsync(x => x.PostId == id);
      Post post = await UnitOfWork.Posts.FirstAsync(x => x.Id == id);
      UnitOfWork.Posts.Delete(post);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<List<PostResponseModel>> GetPostsByPetIdAsync(Guid petId)
    {
      List<Post> posts = await UnitOfWork.Posts
        .Include(x => x.Images)
        .Where(x => x.PetId == petId)
        .ToListAsync();

      var result = Mapper.Map<List<PostResponseModel>>(posts);
      foreach (var post in result)
      {
        UserContextModel userContext = await GetUserContextAsync(post.CreatorId);
        post.UserImage = userContext.Image;
        post.UserName = userContext.Name;
        post.IsLiked = await UnitOfWork.Likes.AnyAsync(x => x.PostId == post.Id && x.UserId == UserContext.Id);
        post.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.PostId == post.Id);
      };
      return result;
    }

    public async Task<int> LikePostAsync(Guid postId)
    {
      Post post = await UnitOfWork.Posts
        .AsTracking()
        .FirstAsync(x => x.Id == postId);
      Like? like = await UnitOfWork.Likes
        .Where(x => x.PostId == postId && x.UserId == UserContext.Id)
        .FirstOrDefaultAsync();
      if (like != null)
      {
        post.Like -= 1;
        UnitOfWork.Likes.Delete(like);
      }
      else
      {
        post.Like += 1;
        await UnitOfWork.Likes.CreateAsync(new Like()
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

