namespace Petopia.BackgroundJobs.Interfaces
{
  public interface IElasticsearchJobService
  {
    public void SyncData();
    public void InitSyncDataCollections();
  }
}