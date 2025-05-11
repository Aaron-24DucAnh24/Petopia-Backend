namespace Petopia.Business.Models.Exceptions
{
  public class InvalidRegisterTokenException : DomainException
  {
    public InvalidRegisterTokenException() : base("Register token is invalid")
    {
      ErrorCode = DomainErrorCode.INVALID_REGISTER_TOKEN;
    }
  }
}