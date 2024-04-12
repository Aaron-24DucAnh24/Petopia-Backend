using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
	public class OwnedPetException : DomainException
	{
		public OwnedPetException() : base("Cannot adopt your own pet")
		{
			ErrorCode = DomainErrorCode.ADOPT_OWNED_PET;
		}
	}
}

