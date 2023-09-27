namespace Petopia.Business.Models.Exceptions
{
  public class NotFoundEmailException : DomainException
  {
    public NotFoundEmailException() : base("Email does not exist")
    {
    }
  }
}