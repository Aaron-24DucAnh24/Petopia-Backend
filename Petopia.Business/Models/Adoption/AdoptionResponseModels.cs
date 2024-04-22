using Petopia.Data.Enums;

namespace Petopia.Business.Models.Adoption
{
	public class DetailAdoptionFormResponseModel
	{
		public Guid Id { get; set; }
		public Guid PetId { get; set; }
		public Guid AdopterId { get; set; }
		public DateTimeOffset IsCreatedAt { get; set; }
		public AdoptStatus Status { get; set; }
		public AdoptHouseType HouseType { get; set; }
		public AdoptDelayDuration DelayDuration { get; set; }
		public string Message { get; set; } = null!;
		public string Address { get; set; } = null!;
		public bool IsOwnerBefore { get; set; }

		public string AdopterName { get; set; } = null!;
		public string AdopterPhone { get; set; } = null!;
		public string AdopterEmail { get; set; } = null!;
		public UserRole AdopterRole { get; set; }
	}

	public class AdoptionFormResponseModel
	{
		public Guid Id { get; set; }
		public DateTimeOffset LastUpdatedAt { get; set; }
		public bool IsSeen { get; set; }
		public AdoptStatus Status { get; set; }
		public string PetName { get; set; } = null!;
		public string AdopterName { get; set; } = null!;
	}
}

