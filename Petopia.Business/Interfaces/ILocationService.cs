using Petopia.Business.Models.Location;

namespace Petopia.Business.Interfaces
{
  public interface ILocationService
  {
    Task<List<LocationResponseModel>> GetLocation(LocationRequestModel request);
  }
}