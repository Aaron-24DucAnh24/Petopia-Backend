using System.Data;
using System.Security.Authentication;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using PetAdoption.Business.Utils;

namespace PetAdoption.API.Middlewares
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
        int code = 500;

        switch (exception)
        {
          case SecurityTokenExpiredException _:
            code = 401;
            message = "Expired security token";
            break;

          case UnauthorizedAccessException _:
            code = 401;
            message = "Unauthorized";
            break;

          case InvalidJwtException _:
          case SecurityTokenValidationException _:
            code = 401;
            message = "Invalid security token";
            break;

          case InvalidCredentialException _:
            code = 400;
            message = "Invalid credential";
            break;

          case DuplicateNameException _:
            code = 400;
            message = "Duplication";
            break;

          default:
            if (string.IsNullOrEmpty(message))
            {
              message = "Some unknown error occurred";
            }
            break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        _logger.LogInformation("{Message}", message);
        await context.Response.WriteAsync(ResponseUtil.GetErrorResponse(message, code));
      }
    }
  }
}