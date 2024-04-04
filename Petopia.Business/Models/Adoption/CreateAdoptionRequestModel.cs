using Petopia.Data.Enums;

namespace Petopia.Business.Models.Adoption
{
	public class CreateAdoptionRequestModel : BaseAdoptionRequestModel
	{
		public Guid PetId { get; set; }
	}

	public class UpdateAdoptionRequestModel : BaseAdoptionRequestModel
	{
		public Guid AdoptionFormId { get; set; }
	}

	public class BaseAdoptionRequestModel
	{
		public AdoptHouseType HouseType { get; set; }
		public AdoptDelayDuration DelayDuration { get; set; }
		public string Message { get; set; } = null!;
		public bool IsOwnerBefore { get; set; }
	}
}