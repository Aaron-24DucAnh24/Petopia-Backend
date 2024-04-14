using Petopia.Business.Models.Pet;
using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
  public class GetUserDetailsResponseModel
  {
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Image { get; set; } = null!;
    public UserRole UserRole { get; set; }
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
		public string ProvinceCode { get; set; } = null!;
		public string DistrictCode { get; set; } = null!;
		public string WardCode { get; set; } = null!;
		public string Street { get; set; } = null!;
	}

  public class CurrentIndividualResponseModel : GetUserDetailsResponseModel
	{
    public CurrentIndividualAttributesResponseModel Attributes { get; set; } = null!;
  }

  public class CurrentOrganizationResponseModel : GetUserDetailsResponseModel
	{
    public CurrentOrganizationAttributesResponseModel Attributes { get; set; } = null!;
	}
}