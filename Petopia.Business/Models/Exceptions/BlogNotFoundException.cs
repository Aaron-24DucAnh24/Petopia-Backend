namespace Petopia.Business.Models.Exceptions
{
  public class BlogNotFoundException : DomainException
  {
    public BlogNotFoundException() : base("The blog is not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_BLOG;
    }
  }
}
