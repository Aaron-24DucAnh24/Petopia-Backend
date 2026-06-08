namespace Petopia.Business.Models.Exceptions
{
  public class UpgradeRequestNotFoundException : DomainException
  {
    public UpgradeRequestNotFoundException() : base("The upgrade request is not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_UPGRADE_REQUEST;
    }
  }
}
