namespace Petopia.Business.Models.Exceptions
{
  public class PostNotFoundException : DomainException
  {
    public PostNotFoundException() : base("The post is not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_POST;
    }
  }
}
