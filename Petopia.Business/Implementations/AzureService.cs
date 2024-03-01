using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
  public class AzureService : BaseService, IAzureService
  {
    public AzureService(
      IServiceProvider provider, 
      ILogger<AzureService> logger
    ) : base(provider, logger)
    {
    }
  }
}
