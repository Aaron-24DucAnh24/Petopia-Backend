using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Exceptions;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class BlogService : BaseService, IBlogService
  {
    private readonly int ADVERTISEMENT_COUNT = 5;

    private readonly ISearchEngineService _searchEngineService;

    public BlogService(
      IServiceProvider provider,
      ILogger<BlogService> logger
    ) : base(provider, logger)
    {
      _searchEngineService = provider.GetRequiredService<ISearchEngineService>();
    }

    public async Task<Guid> CreateBlogAsync(CreateBlogRequestModel request)
    {
      var blog = await UnitOfWork.Blogs.CreateAsync(new Blog()
      {
        Id = Guid.NewGuid(),
        Title = request.Title,
        Content = request.Content,
        Excerpt = request.Excerpt,
        Category = request.Category,
        Image = request.Image,
        UserId = UserContext.Id,
        IsCreatedAt = DateTimeOffset.Now,
        IsUpdatedAt = DateTimeOffset.Now,
      });
      await UnitOfWork.SaveChangesAsync();

      var createdBlog = await UnitOfWork.Blogs
        .Where(x => x.Id == blog.Id)
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .FirstOrDefaultAsync();
      if (createdBlog is not null)
      {
        await _searchEngineService.InsertUpdateAsync(Constants.MEILISEARCH_INDEX_BLOG, Mapper.Map<BlogSearchModel>(createdBlog));
      }

      return blog.Id;
    }

    public async Task<bool> DeleteBlogAsync(Guid id)
    {
      var blog = await UnitOfWork.Blogs
        .AsTracking()
        .Where(b => b.Id == id && b.UserId == UserContext.Id && !b.IsHidden)
        .FirstOrDefaultAsync()
        ?? throw new BlogNotFoundException();
      blog.IsHidden = true;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();

      await _searchEngineService.DeleteAsync(Constants.MEILISEARCH_INDEX_BLOG, blog.Id.ToString());

      return true;
    }

    public async Task<List<BlogResponseModel>> GetAdvertisementAsync()
    {
      var blogs = await UnitOfWork.Blogs
        .Where(b => b.AdvertisingDate.CompareTo(DateTimeOffset.Now) >= 0)
        .Where(b => !b.IsHidden)
        .ToListAsync();
      var returnBlogs = blogs
        .OrderBy(x => Guid.NewGuid())
        .Take(blogs.Count >= ADVERTISEMENT_COUNT ? ADVERTISEMENT_COUNT : blogs.Count)
        .ToList();
      var result = Mapper.Map<List<BlogResponseModel>>(returnBlogs);

      return result;
    }

    public async Task<BlogDetailResponseModel> GetBlogByIdAsync(Guid id)
    {
      var blog = await UnitOfWork.Blogs
        .AsTracking()
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsHidden)
        ?? throw new BlogNotFoundException();

      blog.View += 1;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();

      var result = Mapper.Map<BlogDetailResponseModel>(blog);

      return result;
    }

    public async Task<PaginationResponseModel<BlogResponseModel>> GetBlogsAsync(PaginationRequestModel<BlogFilterModel> request)
    {
      var result = await _searchEngineService.SearchAsync<BlogResponseModel, BlogFilterModel, BlogSearchModel>(Constants.MEILISEARCH_INDEX_BLOG, request);
      return result;
    }

    public async Task<BlogDetailResponseModel> UpdateBlogAsync(UpdateBlogRequestModel request)
    {
      var blog = await UnitOfWork.Blogs
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id)
        ?? throw new BlogNotFoundException();

      blog.Title = request.Title;
      blog.Content = request.Content;
      blog.Excerpt = request.Excerpt;
      blog.Category = request.Category;
      blog.Image = request.Image;
      blog.IsUpdatedAt = DateTimeOffset.Now;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();

      var result = Mapper.Map<BlogDetailResponseModel>(blog);

      return result;
    }
  }
}

