using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Utils;
using System.Net;

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
                int errorCode;

                switch (exception)
                {
                    case SecurityTokenExpiredException _:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Expired security token";
                        break;

                    case UnauthorizedAccessException _:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Unauthorized";
                        break;

                    case SecurityTokenValidationException _:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Invalid security token";
                        break;

                    case ForbiddenAccessException _:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = exception.Message;
                        break;

                    case DomainException _:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        message = exception.Message;
                        break;

                    default:
                        if (string.IsNullOrEmpty(message))
                        {
                            message = "Some unknown error occurred";
                        }
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                if (exception is DomainException)
                {
                    DomainException domainException = (DomainException)exception;
                    errorCode = domainException.ErrorCode;
                }
                else
                {
                    errorCode = statusCode;
                }
                await context.CreateJsonResponseAsync(statusCode, errorCode, message);
                _logger.LogError("{Message}", message);
            }
        }
    }
}