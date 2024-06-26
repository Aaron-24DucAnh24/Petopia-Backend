using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Utils
{
  public static class ResponseUtils
  {
    public static OkObjectResult OkResult<T>(T data)
    {
      try
      {
        if (data != null && data.GetType().GetGenericTypeDefinition() == typeof(PaginationResponseModel<>))
        {
          return new OkObjectResult(data);
        }
      }
      catch (System.Exception) { };

      return new OkObjectResult(new ApiResponseModel<T>()
      {
        Data = data
      });
    }

    public static async Task CreateJsonResponseAsync(this HttpContext context, int statusCode, int errorCode, string errorMessage)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;
      await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiErrorResponseModel()
      {
        ErrorCode = errorCode,
        ErrorMessage = errorMessage
      },
      new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      }));
    }
  }
}