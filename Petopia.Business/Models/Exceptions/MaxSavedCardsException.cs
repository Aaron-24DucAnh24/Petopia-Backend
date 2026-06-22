namespace Petopia.Business.Models.Exceptions
{
  public class MaxSavedCardsException : DomainException
  {
    public MaxSavedCardsException() : base("Maximum 5 saved cards allowed")
    {
      ErrorCode = DomainErrorCode.MAX_SAVED_CARDS;
    }
  }
}
