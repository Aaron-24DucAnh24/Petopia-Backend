namespace Petopia.Business.Interfaces
{
  public interface ISearchEngineService
  {
    public ValueTask<T[]> SearchAsync<T>(string index, string query);
    public ValueTask<T> InsertUpdateAsync<T>(string index, T entity);
    public ValueTask<bool> DeleteAsync(string index, string[] entityIds);
    public ValueTask SyncDataAsync(bool isClean = false);
  }
}