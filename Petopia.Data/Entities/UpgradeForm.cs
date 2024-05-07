#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class UpgradeForm
  {
    public Guid Id { get; set; }
    public string EntityName { get; set; }
    public string OrganizationName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PrivinceCode { get; set; }
    public string DistrictCode { get; set; }
    public string WardCode { get; set; }
    public string Street { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public string TaxCode { get; set; }
    public OrganizationType Type { get; set; }
    public string Description { get; set; }
    public DateTimeOffset IsCreatedAt { get; set; }
    public UpgradeStatus Status { get; set; }

    public User User { get; set; }
  }
}