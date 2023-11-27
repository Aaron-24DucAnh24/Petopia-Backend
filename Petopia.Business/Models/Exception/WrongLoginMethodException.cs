using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class WrongLoginMethodException : DomainException
  {
    public WrongLoginMethodException() : base("Wrong login type")
    {
      ErrorCode = DomainErrorCode.WRONG_LOGIN_TYPE;
    }
  }
}