namespace Petopia.Business.Interfaces
{
  public interface ICacheProvider
  {
    T Set<T>(string key, T value, Double Expiration);
    T? Get<T>(string key);
    bool Remove(string key);
    ValueTask<IEnumerable<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, TimeSpan? cacheDuration = null);
  }
}