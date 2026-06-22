namespace Petopia.Business.Models.Exceptions
{
  public class VaultCardFailedException : DomainException
  {
    public VaultCardFailedException() : base("Cannot save card")
    {
      ErrorCode = DomainErrorCode.VAULT_CARD_FAILED;
    }
  }
}
