using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class PaymentFailureException : DomainException
  {
    public PaymentFailureException() : base("Cannot create payment")
    {
      ErrorCode = DomainErrorCode.CANNOT_CREATE_PAYMENT;
    }
  }
}