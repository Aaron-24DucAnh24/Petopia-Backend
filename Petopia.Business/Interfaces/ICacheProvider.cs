namespace Petopia.Business.Interfaces
{
  public interface ICacheProvider
  {
    bool Remove(string key);
    ValueTask<IEnumerable<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, TimeSpan? cacheDuration = null);
  }
}