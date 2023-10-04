using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Utils
{
  public static class ResponseUtils
  {
    public static async Task CreateJsonResponseAsync(this HttpContext context, int statusCode, string errorMessage)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;
      await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiErrorResponseModel()
      {
        StatusCode = statusCode,
        ErrorMessage = errorMessage
      }));
    }
  }
}