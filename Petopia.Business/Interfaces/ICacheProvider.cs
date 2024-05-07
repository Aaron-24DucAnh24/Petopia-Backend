namespace Petopia.Business.Interfaces
{
  public interface ICacheProvider
  {
    T Set<T>(string key, T value, Double Expiration);
    T? Get<T>(string key);
    bool Remove(string key);
    ValueTask<List<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, double days);
  }
}