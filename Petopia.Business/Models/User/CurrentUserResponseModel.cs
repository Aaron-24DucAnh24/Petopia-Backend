using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class CurrentUserResponseModel
  {
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Image { get; set; } = null!;
    public UserRole UserRole { get; set; }
  }

  public class CurrentIndividualResponseModel : CurrentUserResponseModel
  {
    public CurrentIndividualAttributesResponseModel Attributes { get; set; } = null!;
  }

  public class CurrentOrganizationResponseModel : CurrentUserResponseModel
  {
    public CurrentOrganizationAttributesResponseModel Attributes { get; set; } = null!;
  }
}