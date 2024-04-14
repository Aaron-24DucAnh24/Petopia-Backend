using Petopia.Data.Enums;

#nullable disable

namespace Petopia.Data.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Image { get; set; }
		public string Address { get; set; }
		public string ProvinceCode { get; set; }
		public string DistrictCode { get; set; }
		public string WardCode { get; set; }
		public string Street { get; set; }
		public UserRole Role { get; set; }
		public DateTimeOffset IsCreatedAt { get; set; }
		public string ResetPasswordToken { get; set; }
		public DateTimeOffset ResetPasswordTokenExpirationDate { get; set; }
		public bool IsDeactivated { get; set; }
		public string Phone { get; set; }

		public UserConnection UserConnection { get; set; }
		public UserIndividualAttributes UserIndividualAttributes { get; set; }
		public UserOrganizationAttributes UserOrganizationAttributes { get; set; }
		public List<Pet> Pets { get; set; }
		public List<AdoptionForm> AdoptionForms { get; set; }
		public List<Notification> Notifications { get; set; }
	}
}