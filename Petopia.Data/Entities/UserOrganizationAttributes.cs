#nullable disable

using Petopia.Data.Enums;

namespace Petopia.Data.Entities
{
  public class UserOrganizationAttributes
  {
    public Guid Id { get; set; }
    public string OrganizationName { get; set; }
    public string WebSite { get; set; }
    public OrganizationType OrganizationType { get; set; }
    public User User { get; set; }
  }
}