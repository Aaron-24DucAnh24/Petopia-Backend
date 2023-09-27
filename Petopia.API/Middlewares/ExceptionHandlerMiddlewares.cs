using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Models.Exceptions;

namespace Petopia.API.Middlewares
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _requestDelegate;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlerMiddleware> logger)
    {
      _requestDelegate = requestDelegate;
      _logger = logger;
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
        int statusCode;

        switch (exception)
        {
          case SecurityTokenExpiredException _:
            statusCode = 401;
            message = "Expired security token";
            break;

          case UnauthorizedAccessException _:
            statusCode = 401;
            message = "Unauthorized";
            break;

          case SecurityTokenValidationException _:
            statusCode = 401;
            message = "Invalid security token";
            break;

          case DomainException _:
            statusCode = 400;
            message = exception.Message;
            break;

          default:
            if (string.IsNullOrEmpty(message))
            {
              message = "Some unknown error occurred";
            }
            statusCode = 500;
            break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        _logger.LogInformation("{Message}", message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(message));
      }
    }
  }
}