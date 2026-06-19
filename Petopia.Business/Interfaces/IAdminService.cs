using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Interfaces
{
  public interface IAdminService
  {
    Task<AdminStatisticsResponseModel> GetStatisticsAsync();

    Task<PaginationResponseModel<ManagementUserResponseModel>> GetUsersAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> DeactivateUserAsync(Guid id);
    Task<bool> ActivateUserAsync(Guid id);

    Task<PaginationResponseModel<ManagementPetResponseModel>> GetPetsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> DeactivatePetAsync(Guid id);
    Task<bool> ActivatePetAsync(Guid id);

    Task<PaginationResponseModel<ManagementPostResponseModel>> GetPostsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> DeactivatePostAsync(Guid id);
    Task<bool> ActivatePostAsync(Guid id);

    Task<PaginationResponseModel<ManagementBlogResponseModel>> GetBlogsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> DeactivateBlogAsync(Guid id);
    Task<bool> ActivateBlogAsync(Guid id);

    Task<PaginationResponseModel<ManagementPaymentResponseModel>> GetPaymentsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);

    Task<PaginationResponseModel<ManagementReportResponseModel>> GetReportsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> ResolveReportAsync(ResolveReportRequestModel request);

    Task<PaginationResponseModel<ManagementUpgradeResponseModel>> GetUpgradeRequestsAsync(
      PaginationRequestModel<AdminSearchFilterModel> request);
    Task<bool> ApproveUpgradeRequestAsync(Guid id);
    Task<bool> RejectUpgradeRequestAsync(Guid id);
  }
}
