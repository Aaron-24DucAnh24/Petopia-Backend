namespace Petopia.Business.Models.Exceptions
{
  public class IncorrectEmailException : DomainException
  {
    public IncorrectEmailException() : base("Provided email is not correct")
    {
    }
  }
}