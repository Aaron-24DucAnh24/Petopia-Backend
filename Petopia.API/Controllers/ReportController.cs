using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Report;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Report")]
  public class ReportController : ControllerBase
  {
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
      _reportService = reportService;
    }

    [HttpPost("PreReport")]
    [Authorize]
    public async Task<ActionResult<bool>> PreReport([FromBody] PreReportRequestModel request)
    {
      return ResponseUtils.OkResult(await _reportService.PreReportAsync(request));
    }

    [HttpPost("Report")]
    [Authorize]
    public async Task<ActionResult<bool>> Report([FromBody] ReportRequestModel request)
    {
      return ResponseUtils.OkResult(await _reportService.ReportAsync(request));
    }
  }
}