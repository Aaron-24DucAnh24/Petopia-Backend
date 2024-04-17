using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
	public class UpgradeAccountRequestModel
	{
		public string EntityName { get; set; } = null!;
		public string OrganizationName { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public string PrivinceCode { get; set; } = null!;
		public string DistrictCode { get; set; } = null!;
		public string WardCode { get; set; } = null!;
		public string Street { get; set; } = null!;
		public string Website { get; set; } = null!;
		public string TaxCode { get; set; } = null!;
		public OrganizationType Type { get; set; }
		public string Description { get; set; } = null!;
	}
}

