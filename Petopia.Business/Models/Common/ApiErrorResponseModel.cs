namespace Petopia.Business.Models.Common
{
  public class ApiErrorResponseModel
  {
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; } = null!;
  }
}