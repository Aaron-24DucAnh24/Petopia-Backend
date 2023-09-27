namespace Petopia.Business.Models.Exceptions
{
  public class InvalidPasswordToken : DomainException
  {
    public InvalidPasswordToken() : base("Reset password token is expired")
    {
    }
  }
}