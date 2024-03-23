namespace Petopia.Business.Models.Exceptions
{
  public class ForbiddenAccessException : DomainException
  {
    public ForbiddenAccessException() : base("Forbidden")
    {
    }
  }
}