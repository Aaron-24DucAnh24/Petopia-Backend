namespace Petopia.Business.Models.Exceptions
{
  public class EmailTemplateNotFoundException : DomainException
  {
    public EmailTemplateNotFoundException() : base("Email template not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_EMAIL_TEMPLATE;
    }
  }
}
