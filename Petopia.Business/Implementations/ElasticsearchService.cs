using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
  public class ElasticsearchService : BaseService, IElasticsearchService
  {
    private readonly ElasticsearchClient _elasticsearchClient;
    public ElasticsearchService(
      IServiceProvider provider,
      ILogger<ElasticsearchService> logger,
      ElasticsearchClientSettings settings
    ) : base(provider, logger)
    {
      _elasticsearchClient = new ElasticsearchClient(settings);
    }

    public async Task SyncDataAsync()
    {
      var response = _elasticsearchClient.Ping();
      Console.WriteLine(response.IsSuccess());
    }
  }
}