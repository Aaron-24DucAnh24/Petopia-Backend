using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Location;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class LocationService : BaseService, ILocationService
  {
    private const double LOCATION_CACHING_DAYS = (double)30 / 60 / 24;

    public LocationService(
      IServiceProvider provider,
      ILogger<LocationService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<List<LocationResponseModel>> GetLocation(LocationRequestModel request)
    {
      List<LocationResponseModel> result;
      switch (request.Level)
      {
        case 1:
          IQueryable<Province> provinceQuery = UnitOfWork.Provinces.AsQueryable();
          List<Province>? provinces = await CacheManager.Instance.GetOrSetAsync(provinceQuery, "provinces", LOCATION_CACHING_DAYS);
          result = Mapper.Map<List<LocationResponseModel>>(provinces);
          break;

        case 2:
          List<District> districts = await UnitOfWork.Districts
            .Where(x => x.ParentCode == request.Code)
            .ToListAsync();
          result = Mapper.Map<List<LocationResponseModel>>(districts);
          break;

        default:
          List<Ward> wards = await UnitOfWork.Wards
            .Where(x => x.ParentCode == request.Code)
            .ToListAsync();
          result = Mapper.Map<List<LocationResponseModel>>(wards);
          break;
      }
      return result;
    }
  }
}