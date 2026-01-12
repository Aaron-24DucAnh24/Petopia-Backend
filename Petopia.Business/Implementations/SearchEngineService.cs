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

    public async ValueTask<PaginationResponseModel<TResult>> SearchAsync<TResult, TRequest, TSearch>(
      string index,
      PaginationRequestModel<TRequest> request)
    {
      var indexInstance = _meilisearchClient.Index(index);
      var sortString = CreateSortString(request.OrderBy);
      var filterString = CreateFilterString(request.Filter);
      var totalNumber = (await indexInstance.SearchAsync<TSearch>(
        string.Empty,
        new SearchQuery
        {
          Limit = 0,
          Filter = filterString,
        })).Hits.Count;

      var data = (await indexInstance.SearchAsync<TSearch>(
        string.Empty,
        new SearchQuery
        {
          Limit = request.PageSize,
          Offset = request.PageIndex - 1,
          Sort = sortString,
          Filter = filterString,
          Q = (request.Filter as dynamic).Text,
        })).Hits.ToList();

      var result = new PaginationResponseModel<TResult>
      {
        TotalNumber = 0,
        PageNumber = (int)Math.Ceiling((double)totalNumber / request.PageSize),
        PageIndex = request.PageIndex,
        PageSize = request.PageSize,
        Data = Mapper.Map<List<TResult>>(data),
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
            await indexInstance.AddDocumentsAsync(Mapper.Map<List<PetSearchModel>>(pets));
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
            await indexInstance.AddDocumentsAsync(Mapper.Map<List<BlogSearchModel>>(blogs));
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

    private static string CreateFilterString<T>(T filter)
    {
      var filters = new List<string>();
      var type = typeof(T);
      foreach (var prop in type.GetProperties())
      {
        if (prop.Name == "Text") continue;

        var value = prop.GetValue(filter);
        if (value == null) continue;

        if (value is System.Collections.IEnumerable list)
        {
          var terms = new List<string>();
          foreach (var item in list)
          {
            if (item == null) continue;

            if (item is string s)
            {
              terms.Add($"{prop.Name} = \"{Escape(s)}\"");
            }
            else
            {
              terms.Add($"{prop.Name} = {item}");
            }
          }

          if (terms.Count > 0)
          {
            filters.Add($"({string.Join(" OR ", terms)})");
          }
        }
      }

      var result = string.Join(" AND ", filters);
      return result;
    }

    private static string Escape(string value)
    {
      return value.Replace("\"", "\\\"");
    }

    private static string[]? CreateSortString(string? sort)
    {
      if (string.IsNullOrWhiteSpace(sort))
      {
        return null;
      }

      return sort.ToLower() switch
      {
        Constants.SORT_KEY_NEWEST => new[] { "IsCreatedAt:desc" },
        Constants.SORT_KEY_POPULAR => new[] { "View:desc" },
        _ => null
      };
    }
  }
}