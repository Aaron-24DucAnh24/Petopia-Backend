using Meilisearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Pet;
using Petopia.Business.Models.Post;
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

    public async ValueTask<PaginationResponseModel<TResult>> SearchAsync<TResult, TRequest>(
      string index,
      PaginationRequestModel<TRequest> request)
    {
      var indexInstance = _meilisearchClient.Index(index);
      var sortString = CreateSortString(request.OrderBy);
      var filterString = CreateFilterString(request.Filter);
      var totalNumber = (await indexInstance.SearchAsync<TResult>(
        string.Empty,
        new SearchQuery
        {
          Limit = 0,
          Filter = filterString,
        })).Hits.Count;

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
            Sort = sortString,
            Filter = filterString,
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
      };

      await DeleteUnusedIndexesAsync(indexes);

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
              .Include(x => x.Images)
              .Include(x => x.Owner)
              .Where(pet => !pet.IsDeleted)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(Mapper.Map<List<PetResponseModel>>(pets));
            break;

          case Constants.MEILISEARCH_INDEX_POST:
            var posts = await UnitOfWork.Posts
              .Include(x => x.Images)
              .Where(post => !post.IsDeleted)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(Mapper.Map<List<PostResponseModel>>(posts));
            break;

          case Constants.MEILISEARCH_INDEX_BLOG:
            var blogs = await UnitOfWork.Blogs
              .Where(blog => !blog.IsHidden)
              .Include(x => x.User)
              .ThenInclude(x => x.UserOrganizationAttributes)
              .Where(x => !x.IsHidden)
              .ToListAsync();
            await indexInstance.AddDocumentsAsync(Mapper.Map<List<BlogResponseModel>>(blogs));
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

    private async Task DeleteUnusedIndexesAsync(string[] allowedIndexes)
    {
      var allIndexes = await _meilisearchClient.GetAllIndexesAsync();

      foreach (var index in allIndexes.Results)
      {
        if (!allowedIndexes.Contains(index.Uid))
        {
          await _meilisearchClient.DeleteIndexAsync(index.Uid);
        }
      }
    }

  }
}