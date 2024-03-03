using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Location;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Location")]
  public class LocationController : ControllerBase
  {
    private readonly ILocationService _locationService;

    public LocationController(
      ILocationService locationService
    )
    {
      _locationService = locationService;
    }

    [HttpGet("")]
    [Authorize]
    public async Task<ActionResult<List<LocationResponseModel>>> Get([FromQuery] LocationRequestModel request)
    {
      return ResponseUtils.OkResult(await _locationService.GetLocation(request));
    }
  }
}