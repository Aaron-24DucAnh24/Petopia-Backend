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
        IsCreatedAt = DateTimeOffset.Now,
      });
      await UnitOfWork.SaveChangesAsync();
      return blog.Id;
    }

    public async Task<BlogDetailResponseModel> GetBlogByIdAsync(Guid id)
    {
      Blog blog = await UnitOfWork.Blogs
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsHidden)
        ?? throw new BlogNotFoundException();

      blog.View += 1;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();

      return Mapper.Map<BlogDetailResponseModel>(blog);
    }

    public async Task<PaginationResponseModel<BlogResponseModel>> GetBlogsAsync(PaginationRequestModel request)
    {
      IQueryable<Blog> query = UnitOfWork.Blogs
        .Where(x => !x.IsHidden)
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

