using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class ElasticsearchService : BaseService, IElasticsearchService
  {
    private readonly ElasticsearchClient _elasticsearchClient;
    public ElasticsearchService(
      IServiceProvider provider,
      ILogger<ElasticsearchService> logger,
      ElasticsearchClientSettings settings
    ) : base(provider, logger)
    {
      _elasticsearchClient = new ElasticsearchClient(settings);
    }

    public async Task InitSyncDataCollectionsAsync()
    {
      var users = await UnitOfWork.Users.ToListAsync();
      foreach (var user in users)
      {
        await UnitOfWork.SyncDataCollections.CreateAsync(new SyncDataCollection()
        {
          CollectionId = Guid.NewGuid(),
          ItemId = user.Id,
          Index = ElasticsearchIndex.USERS
        });
      }
      await UnitOfWork.SaveChangesAsync();
      await SyncDataAsync();
    }

    public async Task SyncDataAsync()
    {
      var syncCollections = UnitOfWork.SyncDataCollections
        .AsTracking()
        .Where(x => x.Status != SyncDataStatus.Success)
        .AsEnumerable()
        .GroupBy(x => x.Index);
      foreach (var syncCollection in syncCollections)
      {
        switch (syncCollection.Key)
        {
          case ElasticsearchIndex.USERS:
            await SyncUsersAsync(syncCollection.ToList());
            break;
          default:
            break;
        }
      }
      await UnitOfWork.SaveChangesAsync();
    }

    private async Task SyncUsersAsync(List<SyncDataCollection> collections)
    {
      var indexingCollections = collections.Where(x => x.Action == SyncDataAction.Index).ToList();
      var deletingCollections = collections.Where(x => x.Action == SyncDataAction.Delete).ToList();
      var indexingUsers = await UnitOfWork.Users
        .Where(x => indexingCollections.Select(y => y.ItemId).Contains(x.Id))
        .ToListAsync();
      foreach (var user in indexingUsers)
      {
        var response = await _elasticsearchClient.IndexAsync(new IndexRequest<User>(user, ElasticsearchIndex.USERS, user.Id));
        indexingCollections.Where(x => x.ItemId == user.Id)
          .First().Status = response.IsSuccess() ? SyncDataStatus.Success : SyncDataStatus.Fail;
      }
      foreach (var collection in deletingCollections)
      {
        var response = _elasticsearchClient.DeleteAsync(new DeleteRequest(ElasticsearchIndex.USERS, collection.ItemId));
        collection.Status = response.IsCompletedSuccessfully ? SyncDataStatus.Success : SyncDataStatus.Fail;
      }
    }
  }
}