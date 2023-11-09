namespace Petopia.Business.Models.User
{
  public class CurrentIndividualAttributesResponseModel
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
  }

  public class CurrentOrganizationAttributesResponseModel
  {
    public string OrganizationName { get; set; } = null!;
    public string WebSite { get; set; } = null!;
  }
}