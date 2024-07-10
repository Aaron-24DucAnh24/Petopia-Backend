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
using Petopia.Business.Models.User;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class BaseService
  {
    protected readonly IServiceProvider ServiceProvider;
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
      ServiceProvider = provider;
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

    protected async Task<UserContextModel> GetUserContextAsync(Guid userId)
    {
      UserContextModel result = new();
      User user = await UnitOfWork.Users
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .FirstAsync(x => x.Id == userId);
      string userName = user.Role == UserRole.Organization
        ? user.UserOrganizationAttributes.OrganizationName
        : string.Join(" ", user.UserIndividualAttributes.FirstName, user.UserIndividualAttributes.LastName);
      result.Image = user.Image;
      result.Role = user.Role;
      result.Email = HashUtils.DecryptString(user.Email);
      result.Name = userName;
      result.Id = userId;
      return result;
    }

    protected async Task<PaginationResponseModel<TResult>>
    PagingAsync<TResult, TQuery>(IQueryable<TQuery> query, PaginationRequestModel model)
    {
      PaginationResponseModel<TResult> result = new()
      {
        TotalNumber = await query.CountAsync(),
        PageIndex = model.PageIndex,
        PageSize = model.PageSize
      };
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

    protected PaginationResponseModel<TResult>
    ListPaging<TResult, TQuery>(List<TQuery> list, PaginationRequestModel model)
    {
      PaginationResponseModel<TResult> result = new()
      {
        TotalNumber = list.Count,
        PageIndex = model.PageIndex,
        PageSize = model.PageSize
      };
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
        var domains = list.Skip(skipCount).Take(result.PageSize);
        result.Data = Mapper.Map<List<TResult>>(domains);
      }
      return result;
    }
  }
}