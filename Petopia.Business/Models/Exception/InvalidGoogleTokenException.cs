namespace Petopia.Business.Models.Exceptions
{
  public class InvalidGoogleTokenException : DomainException
  {
    public InvalidGoogleTokenException() : base("Invalid Google login token")
    {
    }
  }
}