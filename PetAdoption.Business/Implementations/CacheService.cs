using System.Runtime.Caching;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class CacheService : ICacheService
  {
    private readonly ObjectCache _memoryCache = MemoryCache.Default;

    public T GetData<T>(string key)
    {
      return (T) _memoryCache.Get(key);
    }

    public bool RemoveData(string key)
    {
      if(string.IsNullOrEmpty(key))
      {
        return false;
      }
      _memoryCache.Remove(key);
      return true;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
      if(string.IsNullOrEmpty(key) || value == null)
      {
        return false;
      }
      _memoryCache.Set(key, value, expirationTime);
      return true;
    }
  }
}