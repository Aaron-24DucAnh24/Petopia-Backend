using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Contexts;
using Petopia.Business.Data;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
  public class BaseService
  {
    protected readonly IConfiguration Configuration;
    protected readonly IUserContext UserContext;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger Logger;
    protected readonly ICacheManager CacheManager;
    protected readonly IMapper Mapper;

    public BaseService(
      IServiceProvider provider,
      ILogger logger
    )
    {
      UserContext = provider.GetRequiredService<IUserContext>();
      Configuration = provider.GetRequiredService<IConfiguration>();
      HttpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
      UnitOfWork = provider.GetRequiredService<IUnitOfWork>();
      CacheManager = provider.GetRequiredService<ICacheManager>();
      Mapper = provider.GetRequiredService<IMapper>();
      Logger = logger;
    }
  }
}