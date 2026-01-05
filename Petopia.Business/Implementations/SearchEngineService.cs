using Meilisearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Setting;

namespace Petopia.Business.Implementations
{
  public class SearchEngineService : BaseService, ISearchEngineService
  {
    private readonly MeilisearchClient _meilisearchClient;

    public SearchEngineService(
      IServiceProvider provider,
      ILogger<SearchEngineService> logger,
      MeiliSettingModel meiliSetting
    ) : base(provider, logger)
    {
      _meilisearchClient = new MeilisearchClient(meiliSetting.Host, meiliSetting.ApiKey);
    }

    public async ValueTask<T> InsertUpdateAsync<T>(string index, T entity)
    {
      var indexInstance = _meilisearchClient.Index(index);
      await indexInstance.AddDocumentsAsync(new[] { entity });
      return entity;
    }

    public async ValueTask<PaginationResponseModel<TResult>> SearchAsync<TResult, TRequest>(string index, PaginationRequestModel<TRequest> request)
    {
      var indexInstance = _meilisearchClient.Index(index);

      var totalNumber = 0;


      var result = new PaginationResponseModel<TResult>
      {
        TotalNumber = 0,
        PageNumber = (int)Math.Ceiling((double)totalNumber / request.PageSize),
        PageIndex = request.PageIndex,
        PageSize = request.PageSize,
        Data = (await indexInstance.SearchAsync<TResult>(
          string.Empty,
          new SearchQuery
          {
            Limit = request.PageSize,
            Offset = request.PageIndex - 1,
            Sort = CreateSortString(request.OrderBy),
            Filter = CreateFilterString(request.Filter),
          })).Hits.ToList()
      };

      return result;
    }

    public async ValueTask SyncDataAsync(bool isClean = false)
    {
      var indexes = new string[]
      {
        Constants.MEILISEARCH_INDEX_PET,
        Constants.MEILISEARCH_INDEX_POST,
        Constants.MEILISEARCH_INDEX_BLOG,
        Constants.MEILISEARCH_INDEX_USER,
      };

      foreach (var index in indexes)
      {
        var indexInstance = _meilisearchClient.Index(index);
        if (isClean)
        {
          await indexInstance.DeleteAllDocumentsAsync();
          Logger.LogInformation($"Cleared documents from index: {indexInstance.Uid}");
        }

        switch (indexInstance.Uid)
        {
          case Constants.MEILISEARCH_INDEX_PET:
            var pets = await UnitOfWork.Pets
              .Where(pet => !pet.IsDeleted)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(pets);
            break;

          case Constants.MEILISEARCH_INDEX_POST:
            var posts = await UnitOfWork.Posts
              .Where(post => !post.IsDeleted)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(posts);
            break;

          case Constants.MEILISEARCH_INDEX_USER:
            var users = await UnitOfWork.Users
              .Where(user => !user.IsDeactivated)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(users);
            break;

          case Constants.MEILISEARCH_INDEX_BLOG:
            var blogs = await UnitOfWork.Blogs
              .Where(blog => !blog.IsHidden)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(blogs);
            break;
        }
      }
    }

    public async ValueTask<bool> DeleteAsync(string index, string[] entityIds)
    {
      var indexInstance = _meilisearchClient.Index(index);
      var result = await indexInstance.DeleteDocumentsAsync(entityIds);
      return result.Type == TaskInfoType.DocumentDeletion;
    }

    private string CreateFilterString<T>(T filter)
    {
      return string.Empty;
    }

    private string[] CreateSortString(string? sort)
    {
      return new string[0];
    }
  }
}