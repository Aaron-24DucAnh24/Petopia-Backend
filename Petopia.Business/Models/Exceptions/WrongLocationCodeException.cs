namespace Petopia.Business.Models.Exceptions
{
  public class WrongLocationCodeException : DomainException
  {
    public WrongLocationCodeException() : base("Wrong location code")
    {
      ErrorCode = DomainErrorCode.WRONG_LOCATION_CODE;
    }
  }
}