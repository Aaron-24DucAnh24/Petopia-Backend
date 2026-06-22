namespace Petopia.Business.Models.Exceptions
{
  public class DeleteCardFailedException : DomainException
  {
    public DeleteCardFailedException() : base("Cannot delete card")
    {
      ErrorCode = DomainErrorCode.DELETE_CARD_FAILED;
    }
  }
}
