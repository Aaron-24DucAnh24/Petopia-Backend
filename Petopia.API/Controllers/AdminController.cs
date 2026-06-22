using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Admin")]
  [AdminAuthorize]
  public class AdminController : ControllerBase
  {
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
      _adminService = adminService;
    }

    [HttpGet("Statistics")]
    public async Task<ActionResult<AdminStatisticsResponseModel>> GetStatistics()
    {
      return ResponseUtils.OkResult(await _adminService.GetStatisticsAsync());
    }

    [HttpPost("User/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementUserResponseModel>>> SearchUsers(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetUsersAsync(request));
    }

    [HttpPut("User/{id}/Deactivate")]
    public async Task<ActionResult<bool>> DeactivateUser(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.DeactivateUserAsync(id));
    }

    [HttpPut("User/{id}/Activate")]
    public async Task<ActionResult<bool>> ActivateUser(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.ActivateUserAsync(id));
    }

    [HttpPost("Pet/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementPetResponseModel>>> SearchPets(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetPetsAsync(request));
    }

    [HttpPut("Pet/{id}/Deactivate")]
    public async Task<ActionResult<bool>> DeactivatePet(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.DeactivatePetAsync(id));
    }

    [HttpPut("Pet/{id}/Activate")]
    public async Task<ActionResult<bool>> ActivatePet(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.ActivatePetAsync(id));
    }

    [HttpPost("Post/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementPostResponseModel>>> SearchPosts(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetPostsAsync(request));
    }

    [HttpPut("Post/{id}/Deactivate")]
    public async Task<ActionResult<bool>> DeactivatePost(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.DeactivatePostAsync(id));
    }

    [HttpPut("Post/{id}/Activate")]
    public async Task<ActionResult<bool>> ActivatePost(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.ActivatePostAsync(id));
    }

    [HttpPost("Blog/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementBlogResponseModel>>> SearchBlogs(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetBlogsAsync(request));
    }

    [HttpPut("Blog/{id}/Deactivate")]
    public async Task<ActionResult<bool>> DeactivateBlog(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.DeactivateBlogAsync(id));
    }

    [HttpPut("Blog/{id}/Activate")]
    public async Task<ActionResult<bool>> ActivateBlog(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.ActivateBlogAsync(id));
    }

    [HttpPost("Payment/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementPaymentResponseModel>>> SearchPayments(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetPaymentsAsync(request));
    }

    [HttpPost("Report/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementReportResponseModel>>> SearchReports(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetReportsAsync(request));
    }

    [HttpPut("Report/Resolve")]
    public async Task<ActionResult<bool>> ResolveReport([FromBody] ResolveReportRequestModel request)
    {
      return ResponseUtils.OkResult(await _adminService.ResolveReportAsync(request));
    }

    [HttpPost("Upgrade/Search")]
    public async Task<ActionResult<PaginationResponseModel<ManagementUpgradeResponseModel>>> SearchUpgradeRequests(
      [FromBody] PaginationRequestModel<AdminSearchFilterModel> request)
    {
      return ResponseUtils.OkResult(await _adminService.GetUpgradeRequestsAsync(request));
    }

    [HttpPut("Upgrade/{id}/Approve")]
    public async Task<ActionResult<bool>> ApproveUpgradeRequest(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.ApproveUpgradeRequestAsync(id));
    }

    [HttpPut("Upgrade/{id}/Reject")]
    public async Task<ActionResult<bool>> RejectUpgradeRequest(Guid id)
    {
      return ResponseUtils.OkResult(await _adminService.RejectUpgradeRequestAsync(id));
    }

    [HttpGet("EmailTemplate")]
    public async Task<ActionResult<List<EmailTemplateResponseModel>>> GetEmailTemplates()
    {
      return ResponseUtils.OkResult(await _adminService.GetEmailTemplatesAsync());
    }

  }
}
