using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
	public class AdoptedPetException : DomainException
	{
		public AdoptedPetException() : base("You have sent request for this pet already")
		{
			ErrorCode = DomainErrorCode.READOPT_PET;
		}
	}
}