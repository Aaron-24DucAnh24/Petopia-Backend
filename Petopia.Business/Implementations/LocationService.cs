using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Location;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class LocationService : BaseService, ILocationService
  {
    private enum LocationLevel
    {
      Province = 1,
      District = 2,
      Ward = 3,
    }

    public LocationService(
      IServiceProvider provider,
      ILogger<LocationService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<List<LocationResponseModel>> GetLocation(LocationRequestModel request)
    {
      List<LocationResponseModel> result = new();
      switch (request.Level)
      {
        case (int)LocationLevel.Province:
          IQueryable<Province> provinceQuery = UnitOfWork.Provinces.AsQueryable();
          List<Province>? provinces = await CacheManager.Instance.GetOrSetAsync(
            provinceQuery,
            Constants.CACHE_KEY_PROVINCES,
            Constants.CACHE_TIME_LOCATION);
          result = Mapper.Map<List<LocationResponseModel>>(provinces);
          break;

        case (int)LocationLevel.District:
          List<District> districts = await UnitOfWork.Districts
            .Where(x => x.ParentCode == request.Code)
            .ToListAsync();
          result = Mapper.Map<List<LocationResponseModel>>(districts);
          break;

        case (int)LocationLevel.Ward:
          List<Ward> wards = await UnitOfWork.Wards
            .Where(x => x.ParentCode == request.Code)
            .ToListAsync();
          result = Mapper.Map<List<LocationResponseModel>>(wards);
          break;
      }

      if (result.Any())
      {
        result.Sort((a, b) => a.Name.CompareTo(b.Name));
        result.Insert(0, new LocationResponseModel()
        {
          Name = string.Empty,
          Code = string.Empty,
        });
      }
      return result;
    }
  }
}