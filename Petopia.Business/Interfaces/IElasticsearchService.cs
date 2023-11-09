namespace Petopia.Business.Interfaces
{
  public interface IElasticsearchService
  {
    public Task SyncDataAsync();
    public Task InitSyncDataCollectionsAsync();
  }
}