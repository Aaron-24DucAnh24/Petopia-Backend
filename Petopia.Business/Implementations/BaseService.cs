using System.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Contexts;
using Petopia.Business.Data;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Setting;
using Petopia.Data.Entities;

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

    protected async Task<User> GetUserAttributesAsync()
    {
      return await UnitOfWork.Users
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .FirstAsync(x => x.Id == UserContext.Id);
    }

    protected async Task<PaginationResponseModel<TResult>>
    PagingAsync<TResult, TQuery>(IQueryable<TQuery> query, PaginationRequestModel model)
    {
      var result = new PaginationResponseModel<TResult>();
      result.TotalNumber = await query.CountAsync();
      result.PageIndex = model.PageIndex;
      result.PageSize = model.PageSize != null ? model.PageSize.Value : result.TotalNumber;
      result.PageNumber = model.PageSize == 0 ? 0 : (int)Math.Ceiling((double)result.TotalNumber / result.PageSize);

      if (result.PageNumber < 1)
      {
        result.PageNumber = 1;
      }
      if (result.PageIndex < 1)
      {
        result.PageIndex = 1;
      }
      if (result.PageIndex > result.PageNumber)
      {
        result.PageIndex = result.PageNumber;
      }

      if (result.PageNumber > 0)
      {
        var skipCount = (result.PageIndex - 1) * result.PageSize;
        var domains = await query.Skip(skipCount).Take(result.PageSize).ToListAsync();
        result.Data = Mapper.Map<List<TResult>>(domains);
      }
      return result;
    }
  }
}