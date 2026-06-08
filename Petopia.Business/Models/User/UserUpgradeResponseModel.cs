using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class UserUpgradeResponseModel
  {
    public Guid Id { get; set; }
    public UpgradeStatus Status { get; set; }
    public string EntityName { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string TaxCode { get; set; } = null!;
    public OrganizationType Type { get; set; }
    public string Description { get; set; } = null!;
    public DateTimeOffset IsCreatedAt { get; set; }
  }
}
