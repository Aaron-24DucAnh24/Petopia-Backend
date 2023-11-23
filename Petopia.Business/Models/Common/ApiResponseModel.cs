namespace Petopia.Business.Models.Common
{
  public class ApiResponseModel<T>
  {
    public required T Data { get; set; }
  }
}