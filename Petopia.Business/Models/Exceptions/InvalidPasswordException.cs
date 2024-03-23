using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
	public class InvalidPasswordException : DomainException
	{
		public InvalidPasswordException() : base("New password must be different to the old one")
		{
			ErrorCode = DomainErrorCode.INVALID_PASSWORD;
		}
	}
}