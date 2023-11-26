using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class GoogleRecaptchaTokenException : DomainException
  {
    public GoogleRecaptchaTokenException() : base("Google recaptcha token is expired")
    {
      ErrorCode = DomainErrorCode.EXPIRED_GOOGLE_RECAPTCHA_TOKEN;
    }
  }
}