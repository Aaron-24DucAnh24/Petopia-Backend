using System.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Contexts;
using Petopia.Business.Data;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Setting;

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
    protected readonly AppUrlsSettingModel AppUrls;
    protected readonly IHttpService HttpService;

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
      HttpService = provider.GetRequiredService<IHttpService>();
      Logger = logger;
      AppUrls = Configuration
        .GetSection(AppSettingKey.APP_URLS)
        .Get<AppUrlsSettingModel>()
        ?? throw new ConfigurationErrorsException();
    }
  }
}