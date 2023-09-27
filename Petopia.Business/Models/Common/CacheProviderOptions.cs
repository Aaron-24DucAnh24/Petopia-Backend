namespace Petopia.Business.Models.Common
{
  public class CacheProviderOptions
  {
    private DateTimeOffset? _absoluteExpiration;
    private TimeSpan? _absoluteExpirationRelativeToNow;
    private TimeSpan? _slidingExpiration;

    public DateTimeOffset? AbsoluteExpiration
    {
      get { return _absoluteExpiration; }
      set { _absoluteExpiration = value; }
    }

    public TimeSpan? AbsoluteExpirationRelativeToNow
    {
      get { return _absoluteExpirationRelativeToNow; }
      set
      {
        if (value <= TimeSpan.Zero)
        {
          throw new Exception("AbsoluteExpirationRelativeToNow must be positive");
        }
        else
        {
          _absoluteExpirationRelativeToNow = value;
        }
      }
    }

    public TimeSpan? SlidingExpiration
    {
      get { return _slidingExpiration; }
      set
      {
        if (value <= TimeSpan.Zero)
        {
          throw new Exception("AbsoluteExpirationRelativeToNow must be positive");
        }
        else
        {
          _slidingExpiration = value;
        }
      }
    }
  }
}