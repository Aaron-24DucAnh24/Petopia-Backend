using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class InvalidCredentialException : DomainException
  {
    public InvalidCredentialException() : base("Invalid credential")
    {
      ErrorCode = DomainErrorCode.INCORRECT_CREDENTIAL;
    }
  }
}