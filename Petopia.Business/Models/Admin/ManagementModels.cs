using Petopia.Business.Models.Enums;
using Petopia.Data.Enums;

namespace Petopia.Business.Models.Admin
{
  public class ManagementUserResponseModel
  {
    public Guid Id { get; set; }
    public required string Image { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public UserRole Role { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public bool IsActive { get; set; }
  }

  public class ManagementPetResponseModel
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public PetSpecies Species { get; set; }
    public required string Breed { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public required string OwnerName { get; set; }
    public Guid OwnerId { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }

  public class ManagementPostResponseModel
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Content { get; set; }
    public int Like { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }

  public class ManagementBlogResponseModel
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Title { get; set; }
    public BlogCategory Category { get; set; }
    public int View { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }

  public class ManagementPaymentResponseModel
  {
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public required string BlogTitle { get; set; }
    public required string UserName { get; set; }
    public int Amount { get; set; }
    public int MonthDuration { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }

  public class ManagementReportResponseModel
  {
    public Guid TargetId { get; set; }
    public ReportEntity TargetType { get; set; }
    public int SpamCount { get; set; }
    public int ScamCount { get; set; }
    public int InappropriateCount { get; set; }
    public int OtherCount { get; set; }
    public int TotalCount { get; set; }
    public bool IsResolved { get; set; }
    public DateTimeOffset LastReportAt { get; set; }
  }

  public class ManagementUpgradeResponseModel
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserImage { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string OrganizationName { get; set; }
    public required string EntityName { get; set; }
    public required string Email { get; set; }
    public required string Website { get; set; }
    public required string TaxCode { get; set; }
    public OrganizationType Type { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
  }

  public class AdminSearchFilterModel
  {
    public string? Keyword { get; set; }
    public bool? IsActive { get; set; }
  }

  public class ResolveReportRequestModel
  {
    public Guid TargetId { get; set; }
    public ReportEntity TargetType { get; set; }
  }
}
