namespace Petopia.Business.Models.Exceptions
{
  public class IncorrectPasswordException : DomainException
  {
    public IncorrectPasswordException() : base("Provided password is not correct")
    {
      ErrorCode = DomainErrorCode.INCORRECT_PASSWORD;
    }
  }
}