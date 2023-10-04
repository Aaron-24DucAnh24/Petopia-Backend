namespace Petopia.Business.Models.Exceptions
{
  public class InvalidPasswordTokenException : DomainException
  {
    public InvalidPasswordTokenException() : base("Reset password token is expired")
    {
    }
  }
}