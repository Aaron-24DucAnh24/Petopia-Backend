using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class CurrentIndividualAttributesResponseModel
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
  }

  public class CurrentOrganizationAttributesResponseModel
  {
    public string EntityName { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string TaxCode { get; set; } = null!;
    public OrganizationType Type { get; set; }
    public string Description { get; set; } = null!;
  }
}