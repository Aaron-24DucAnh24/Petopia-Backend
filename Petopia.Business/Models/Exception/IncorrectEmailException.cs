using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class IncorrectEmailException : DomainException
  {
    public IncorrectEmailException() : base("Provided email is not correct")
    {
      ErrorCode = DomainErrorCode.INCORRECT_MAIL;
    }
  }
}