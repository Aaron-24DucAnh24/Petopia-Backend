//using Elastic.Clients.Elasticsearch;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Petopia.Business.Constants;
//using Petopia.Business.Interfaces;
//using Petopia.Data.Entities;
//using Petopia.Data.Enums;

//namespace Petopia.Business.Implementations
//{
//  public class ElasticsearchService : BaseService, IElasticsearchService
//  {
//    private readonly ElasticsearchClient _elasticsearchClient;

//    public ElasticsearchService(
//      IServiceProvider provider,
//      ILogger<ElasticsearchService> logger,
//      ElasticsearchClientSettings settings
//    ) : base(provider, logger)
//    {
//      _elasticsearchClient = new ElasticsearchClient(settings);
//    }

//    public ElasticsearchClient ElasticsearchClient
//    {
//      get
//      {
//        return _elasticsearchClient;
//      }
//    }

//    public async Task InitSyncDataCollectionsAsync()
//    {
//      List<Pet> pets = await UnitOfWork.Pets.ToListAsync();
//      foreach (var pet in pets)
//      {
//        await UnitOfWork.SyncDataCollections.CreateAsync(new SyncDataCollection()
//        {
//          Id = Guid.NewGuid(),
//          ItemId = pet.Id,
//          Index = ElasticsearchIndex.PETS
//        });
//      }
//      await UnitOfWork.SaveChangesAsync();
//      await SyncDataAsync();
//    }

//    public async Task SyncDataAsync()
//    {
//      var syncCollections = UnitOfWork.SyncDataCollections
//        .AsTracking()
//        .Where(x => x.Status != SyncDataStatus.Success)
//        .AsEnumerable()
//        .GroupBy(x => x.Index);
//      foreach (var syncCollection in syncCollections)
//      {
//        switch (syncCollection.Key)
//        {
//          case ElasticsearchIndex.PETS:
//            await SyncPetsAsync(syncCollection.ToList());
//            break;
//          default:
//            break;
//        }
//      }
//      await UnitOfWork.SaveChangesAsync();
//    }

//    private async Task SyncPetsAsync(List<SyncDataCollection> collections)
//    {
//      List<SyncDataCollection> indexingCollections = collections.Where(x => x.Action == SyncDataAction.Index).ToList();
//      List<SyncDataCollection> deletingCollections = collections.Where(x => x.Action == SyncDataAction.Delete).ToList();
//      List<Pet> indexingPets = await UnitOfWork.Pets
//        .Where(x => indexingCollections.Select(y => y.ItemId).Contains(x.Id))
//        .ToListAsync();
//      foreach (var pet in indexingPets)
//      {
//        IndexResponse response = await _elasticsearchClient.IndexAsync(new IndexRequest<Pet>(pet, ElasticsearchIndex.PETS, pet.Id));
//        indexingCollections.Where(x => x.ItemId == pet.Id)
//          .First().Status = response.IsSuccess() ? SyncDataStatus.Success : SyncDataStatus.Fail;
//      }
//      foreach (var collection in deletingCollections)
//      {
//        DeleteResponse response = await _elasticsearchClient.DeleteAsync(new DeleteRequest(ElasticsearchIndex.PETS, collection.ItemId));
//        collection.Status = response.IsSuccess() ? SyncDataStatus.Success : SyncDataStatus.Fail;
//      }
//    }
//  }
//}