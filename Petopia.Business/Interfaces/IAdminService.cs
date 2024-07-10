using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Interfaces
{
  public interface IAdminService
  {
    public Task<DashboardReponseModel> GetDashboardAsync(DashboardRequestModel time);
    public Task<PaginationResponseModel<ManagementUserResponseModel>> GetUsersAsync(PaginationRequestModel<ManagementUserFilter> request);
    public Task<bool> ActivateUserAsync(ActivateRequestModel request);
    public Task<string> CreateAdminAsync(string email);
    public Task<PaginationResponseModel<ManagementPetResponseModel>> GetPetsAsync(PaginationRequestModel request);
    public Task<PaginationResponseModel<ManagementBlogResponseModel>> GetBlogsAsync(PaginationRequestModel request);
    public Task<PaginationResponseModel<ManagementUpgradeResponseModel>> GetUpgradeRequestsAsync(PaginationRequestModel request);
    public Task<string> ActUpgradeRequestAsync(Guid id, bool accepted);
    public Task<PaginationResponseModel<ManagementReportResponseModel>> GetReportsAsync(PaginationRequestModel<ManagementReportFilter> request);
  }
}