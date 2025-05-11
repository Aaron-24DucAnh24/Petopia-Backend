using System.Net;
namespace Petopia.Business.Models.Exceptions
{
  public class DomainException : WebException
  {
    public int ErrorCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public DomainException(string message) : base(message)
    {
    }
  }
}