namespace Petopia.Business.Models.Exceptions
{
  public class PaymentTokenException : DomainException
  {
    public PaymentTokenException() : base("Cannot generate payment token")
    {
    }
  }
}