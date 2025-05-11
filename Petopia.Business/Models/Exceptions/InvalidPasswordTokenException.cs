namespace Petopia.Business.Models.Exceptions
{
  public class InvalidPasswordTokenException : DomainException
  {
    public InvalidPasswordTokenException() : base("Reset password token is invalid")
    {
      ErrorCode = DomainErrorCode.INVALID_PASSWORD_TOKEN;
    }
  }
}