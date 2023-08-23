using System.Text.Json;
using PetAdoption.Business.Models;

namespace PetAdoption.Business.Utils
{
  public class ResponseUtil
  {
    public static string GetErrorResponse<T>(T error, int code)
    {
      return JsonSerializer.Serialize(new ErrorResponse<T>()
      {
        Error = error,
        StatusCode = code.ToString()
      });
    }
  }
}