using Petopia.Data.Enums;

namespace Petopia.Business.Models.Adoption
{
	public class CreateAdoptionRequestModel : BaseAdoptionRequestModel
	{
		public Guid PetId { get; set; }
		public string ProvinceCode { get; set; } = null!;
		public string DistrictCode { get; set; } = null!;
		public string WardCode { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public string Street { get; set; } = null!;
	}

	public class UpdateAdoptionRequestModel : BaseAdoptionRequestModel
	{
		public Guid AdoptionFormId { get; set; }
	}

	public class BaseAdoptionRequestModel
	{
		public AdoptHouseType HouseType { get; set; }
		public AdoptDelayDuration AdoptTime { get; set; }
		public string Message { get; set; } = null!;
		public bool IsOwnerBefore { get; set; }
	}
}