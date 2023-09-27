namespace Petopia.Business.Interfaces
{
  public interface IHttpService
  {
    public void AddHeaders(Dictionary<string, string> headers);
    public Task<TResult?> PostJsonAsync<TBody, TResult>(string uri, TBody data);
    public Task<T?> PostFormAsync<T>(string uri, Dictionary<string, string> data);
    public Task<T?> GetAsync<T>(string uri);
    public Task<T?> GetAsync<T>(string uri, Dictionary<string, string?> query);
  }
}