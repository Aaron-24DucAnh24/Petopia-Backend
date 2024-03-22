using Petopia.Data.Enums;

namespace Petopia.Business.Models.User
{
	public class CurrentUserCoreResponseModel
	{
		public string Id { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Image { get; set; } = null!;
		public UserRole UserRole { get; set; }
		public string Name { get; set; } = null!;
	}
}

