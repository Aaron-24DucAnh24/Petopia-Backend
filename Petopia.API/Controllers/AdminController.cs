using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Email;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Admin")]
  public class AdminController : ControllerBase
  {
    private readonly IAdminService _adminService;
    private readonly IEmailService _emailService;
    private readonly IEmailJobService _emailJobService;

    public AdminController(
      IAdminService adminService,
      IEmailService emailService,
      IEmailJobService emailJobService
    )
    {
      _adminService = adminService;
      _emailService = emailService;
      _emailJobService = emailJobService;
    }

    [HttpPost("Dashboard")]
    [AdminAuthorize]
    public async Task<ActionResult<DashboardReponseModel>> GetDashboard([FromBody] DashboardRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.GetDashboardAsync(request));
    }

    [HttpPost("User/Get")]
    [AdminAuthorize]
    public async Task<ActionResult<PaginationResponseModel<ManagementUserResponseModel>>>
    GetUsers([FromBody] PaginationRequestModel<ManagementUserFilter> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetUsersAsync(request));
    }

    [HttpPut("Activate")]
    [AdminAuthorize]
    public async Task<ActionResult<bool>> ActivateUser([FromBody] ActivateRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.ActivateUserAsync(request));
    }

    [HttpPost("{email}")]
    [AdminAuthorize]
    public async Task<ActionResult<bool>> CreateAdmin(string email)
    {
      string password = await _adminService.CreateAdminAsync(email);
      MailDataModel mailMessage = await _emailService.CreateAdminMailDataAsync(email, password);
      _emailJobService.SendMail(mailMessage);
      return ResponseUtils.OkResult(true);
    }

    [HttpPost("Pet/Get")]
    [AdminAuthorize]
    public async Task<ActionResult<PaginationResponseModel<ManagementPetResponseModel>>>
    GetPets([FromBody] PaginationRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.GetPetsAsync(request));
    }

    [HttpPost("Blog/Get")]
    [AdminAuthorize]
    public async Task<ActionResult<PaginationResponseModel<ManagementBlogResponseModel>>>
    GetBlogs([FromBody] PaginationRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.GetBlogsAsync(request));
    }

    [HttpPost("UpgradeRequest/Get")]
    [AdminAuthorize]
    public async Task<ActionResult<PaginationResponseModel<ManagementUpgradeResponseModel>>>
    GetUpgradeRequests([FromBody] PaginationRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.GetUpgradeRequestsAsync(request));
    }

    [HttpPut("UpgradeRequest/Confirm/{id}")]
    [AdminAuthorize]
    public async Task<ActionResult<bool>>
    ConfirmUpgradeRequest(Guid id)
    {
      string email = await _adminService.ActUpgradeRequestAsync(id, true);
      MailDataModel mailData = await _emailService.CreateUpgradeMailDataAsync(email, true);
      _emailJobService.SendMail(mailData);
      return ResponseUtils.OkResult(true);
    }

    [HttpPut("UpgradeRequest/Reject/{id}")]
    [AdminAuthorize]
    public async Task<ActionResult<bool>>
    RejectUpgradeRequest(Guid id)
    {
      string email = await _adminService.ActUpgradeRequestAsync(id, false);
      MailDataModel mailData = await _emailService.CreateUpgradeMailDataAsync(email, false);
      _emailJobService.SendMail(mailData);
      return ResponseUtils.OkResult(true);
    }

    [HttpPost("Report/Get")]
    [AdminAuthorize]
    public async Task<ActionResult<PaginationResponseModel<ManagementReportResponseModel>>>
    GetReport([FromBody] PaginationRequestModel<ManagementReportFilter> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetReportsAsync(request));
    }
  }
}