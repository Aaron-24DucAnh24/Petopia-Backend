using Petopia.Business.Models.Enums;
using Petopia.Data.Enums;

namespace Petopia.Business.Models.Admin
{
  public class ManagementUserResponseModel
  {
    public Guid Id { get; set; }
    public required string Image { get; set; }
    public required string Name { get; set; }
    public required string OrganizationName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public bool IsDeactivated { get; set; }
    public OrganizationType OrganizationType { get; set; }
  }

  public class ManagementUserFilter
  {
    public UserRole Role { get; set; }
  }

  public class ActivateRequestModel
  {
    public required string Type;
    public Guid Id;
  }

  public class ManagementPetResponseModel
  {
    public Guid Id { get; set; }
    public required string Image { get; set; }
    public required string Name { get; set; }
    public PetSpecies Species { get; set; }
    public required string Breed { get; set; }
    public bool IsAvailable { get; set; }
    public int View { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset IsUpdatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public required string OwnerImage { get; set; }
    public bool IsDeleted { get; set; }
  }

  public class ManagementBlogResponseModel
  {
    public Guid Id { get; set; }
    public required string Image { get; set; }
    public Guid UserId { get; set; }
    public required string UserImage { get; set; }
    public BlogCategory Category { get; set; }
    public required string Title { get; set; }
    public int View { get; set; }
    public bool IsHidden { get; set; }
    public DateTimeOffset AdvertisingDate { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public DateTimeOffset IsUpdatedAt { get; set; }
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

  public class ManagementReportFilter
  {
    public ReportEntity ReportEntity { get; set; }
  }

  public class ManagementReportResponseModel
  {
    public Guid? Id { get; set; }
    public int Spam { get; set; }
    public int Scam { get; set; }
    public int InappropriateContent { get; set; }
    public int Other { get; set; }
  }
}