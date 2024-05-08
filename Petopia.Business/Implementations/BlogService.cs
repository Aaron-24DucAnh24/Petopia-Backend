using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
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

    public BlogService(
      IServiceProvider provider,
      ILogger<BlogService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<Guid> CreateBlogAsync(CreateBlogRequestModel request)
    {
      Blog blog = await UnitOfWork.Blogs.CreateAsync(new Blog()
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
      return blog.Id;
    }

    public async Task<bool> DeleteBlogAsync(Guid id)
    {
      Blog? blog = await UnitOfWork.Blogs
        .AsTracking()
        .Where(b => b.Id == id && b.UserId == UserContext.Id)
        .FirstOrDefaultAsync();
      if (blog == null)
      {
        return false;
      }
      blog.IsHidden = true;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<List<BlogResponseModel>> GetAdvertisementAsync()
    {
      List<Blog> blogs = await UnitOfWork.Blogs
      .Where(b => b.AdvertisingDate.CompareTo(DateTimeOffset.Now) >= 0)
      .Where(b => !b.IsHidden)
      .ToListAsync();

      List<Blog> returnBlogs = blogs
        .OrderBy(x => Guid.NewGuid())
        .Take(blogs.Count >= ADVERTISEMENT_COUNT ? ADVERTISEMENT_COUNT : blogs.Count)
        .ToList();

      return Mapper.Map<List<BlogResponseModel>>(returnBlogs);
    }

    public async Task<BlogDetailResponseModel> GetBlogByIdAsync(Guid id)
    {
      Blog blog = await UnitOfWork.Blogs
        .AsTracking()
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsHidden)
        ?? throw new BlogNotFoundException();

      blog.View += 1;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();

      return Mapper.Map<BlogDetailResponseModel>(blog);
    }

    public async Task<PaginationResponseModel<BlogResponseModel>> GetBlogsAsync(PaginationRequestModel<BlogFilterModel> request)
    {
      IQueryable<Blog> query = UnitOfWork.Blogs
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .Where(x => !x.IsHidden)
        .AsQueryable();
      if (request.Filter.Category != null)
      {
        query = query.Where(x => x.Category == request.Filter.Category);
      }
      if (!string.IsNullOrEmpty(request.OrderBy))
      {
        query = request.OrderBy == OrderKey.NEWEST
        ? query.OrderByDescending(x => x.IsCreatedAt)
        : query.OrderByDescending(x => x.View);
      }

      return await PagingAsync<BlogResponseModel, Blog>(query, request);
    }

    public async Task<PaginationResponseModel<BlogResponseModel>> GetBlogsByUserIdAsync(PaginationRequestModel request)
    {
      IQueryable<Blog> query = UnitOfWork.Blogs
        .Include(x => x.User)
        .ThenInclude(x => x.UserOrganizationAttributes)
        .Where(x => !x.IsHidden && x.UserId == UserContext.Id)
        .AsQueryable();
      if (!string.IsNullOrEmpty(request.OrderBy))
      {
        query = request.OrderBy == OrderKey.NEWEST
        ? query.OrderByDescending(x => x.IsCreatedAt)
        : query.OrderByDescending(x => x.View);
      }

      return await PagingAsync<BlogResponseModel, Blog>(query, request);
    }

    public async Task<BlogDetailResponseModel> UpdateBlogAsync(UpdateBlogRequestModel request)
    {
      Blog blog = await UnitOfWork.Blogs
        .AsTracking()
        .FirstAsync(x => x.Id == request.Id);
      blog.Title = request.Title;
      blog.Content = request.Content;
      blog.Excerpt = request.Excerpt;
      blog.Category = request.Category;
      blog.Image = request.Image;
      blog.IsUpdatedAt = DateTimeOffset.Now;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();
      return Mapper.Map<BlogDetailResponseModel>(blog);
    }
  }
}

