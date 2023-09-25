using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class HttpService : IHttpService
  {
    private readonly HttpClient _httpClient;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient();
    }

    public void AddHeaders(Dictionary<string, string> headers)
    {
      foreach(var headerName in headers.Keys)
      {
        if(_httpClient.DefaultRequestHeaders.Contains(headerName))
        {
          _httpClient.DefaultRequestHeaders.Remove(headerName);
        }
        _httpClient.DefaultRequestHeaders.Add(headerName, headers[headerName]);
      }
    }

    public async Task<T?> GetAsync<T>(string uri)
    {
      var response = await _httpClient.GetAsync(uri);
      return await UnpackAsync<T>(response);
    }

    public async Task<T?> GetAsync<T>(string uri, Dictionary<string, string?> query)
    {
      uri = QueryHelpers.AddQueryString(uri, query);
      var response = await _httpClient.GetAsync(uri);
      return await UnpackAsync<T>(response);
    }

    public async Task<T?> PostFormAsync<T>(string uri, Dictionary<string, string> data)
    {
      var formData = new FormUrlEncodedContent(data);
      var response = await _httpClient.PostAsync(uri, formData);
      return await UnpackAsync<T>(response);
    }

    public async Task<TResult?> PostJsonAsync<TBody, TResult>(string uri, TBody data)
    {
      var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
      var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(uri, content);
      return await UnpackAsync<TResult>(response);
    }

    private async Task<T?> UnpackAsync<T>(HttpResponseMessage response)
    {
      var responseContent = await response.Content.ReadAsStringAsync();
      var result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions()
      {
        PropertyNameCaseInsensitive = true
      });
      return result;
    }
  }
}