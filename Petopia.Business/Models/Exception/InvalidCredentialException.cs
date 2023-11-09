namespace Petopia.Business.Models.Exceptions
{
  public class InvalidCredentialException : DomainException
  {
    public InvalidCredentialException() : base("Invalid credential")
    {
    }
  }
}