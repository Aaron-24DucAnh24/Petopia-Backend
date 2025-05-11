namespace Petopia.Business.Models.Exceptions
{
  public class UsedEmailException : DomainException
  {
    public UsedEmailException() : base("Email has been used")
    {
      ErrorCode = DomainErrorCode.USED_EMAIL;
    }
  }
}