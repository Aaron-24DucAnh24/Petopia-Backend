using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Business.Contexts;
using PetAdoption.Business.Data;
using PetAdoption.Business.Services.Interfaces;

namespace PetAdoption.Business.Services.Implementations
{
  public class BaseService
  {
    protected readonly IConfiguration Configuration;
    protected readonly IUserContext UserContext;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ICacheService CacheService;

    public BaseService(IServiceProvider provider)
    {
      UserContext = provider.GetService<IUserContext>() ?? throw new Exception("Service not found");
      Configuration = provider.GetService<IConfiguration>() ?? throw new Exception("Service not found");
      HttpContextAccessor = provider.GetService<IHttpContextAccessor>() ?? throw new Exception("Service not found");
      CacheService = provider.GetService<ICacheService>() ?? throw new Exception("Service not found");
      UnitOfWork = provider.GetService<IUnitOfWork>() ?? throw new Exception("Service not found");
    }
  }
}