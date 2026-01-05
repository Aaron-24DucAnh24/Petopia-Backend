using Petopia.Business.Models.Common;

namespace Petopia.Business.Interfaces
{
  public interface ISearchEngineService
  {
    public ValueTask<PaginationResponseModel<TResult>> SearchAsync<TResult, TRequest>(string index, PaginationRequestModel<TRequest> request);
    public ValueTask<T> InsertUpdateAsync<T>(string index, T entity);
    public ValueTask<bool> DeleteAsync(string index, string[] entityIds);
    public ValueTask SyncDataAsync(bool isClean = false);
  }
}