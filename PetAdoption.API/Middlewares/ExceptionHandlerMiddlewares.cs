using System.Text.Json;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Models;
using PetAdoption.Business.Utils;

namespace PetAdoption.API.Middlewares
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _requestDelegate;

    public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
    {
      _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _requestDelegate(context);
      }
      catch (Exception exception)
      {
        string message = exception.Message;
        int code = 500;

        if ( message == ExceptionMessage.ACCESS_TOKEN_EXPIRED
          || message == ExceptionMessage.INVALID_ACCESS_TOKEN
          || message == ExceptionMessage.UNAUTHORIZED)
        {
          code = 401; 
        }

        if ( message == ExceptionMessage.INCORRECT_LOGIN_INFO
          || message == ExceptionMessage.DUPLICATE)
        {
          code = 400;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        await context.Response.WriteAsync(ResponseUtil.GetErrorResponse(message, code));
      }
    }
  }
}