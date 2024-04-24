#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class UserOrganizationAttributes
  {
    public Guid Id { get; set; }
    public string EntityName { get; set; }
    public string OrganizationName { get; set; }
    public string Website { get; set; }
    public string TaxCode { get; set; }
    public OrganizationType Type { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }

    public User User { get; set; }
  }
}