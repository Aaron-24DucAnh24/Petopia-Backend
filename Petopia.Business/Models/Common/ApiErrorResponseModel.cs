namespace Petopia.Business.Models.Common
{
  public class ApiErrorResponseModel
  {
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = null!;
  }
}