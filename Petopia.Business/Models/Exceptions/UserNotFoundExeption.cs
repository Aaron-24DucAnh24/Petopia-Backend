using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class UserNotFoundException : DomainException
  {
    public UserNotFoundException() : base("The user is not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_USER;
    }
  }
}