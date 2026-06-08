namespace Petopia.Business.Models.Exceptions
{
  public class UpgradeRequestNotPendingException : DomainException
  {
    public UpgradeRequestNotPendingException() : base("The upgrade request is not in pending status")
    {
      ErrorCode = DomainErrorCode.UPGRADE_REQUEST_NOT_PENDING;
    }
  }
}
