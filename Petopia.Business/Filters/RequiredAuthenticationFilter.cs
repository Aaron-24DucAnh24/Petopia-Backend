using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Petopia.Business.Filters
{
  internal class RequiredAuthenticationFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
      {
        return;
      }
      operation.Security = new List<OpenApiSecurityRequirement>
      {
        new OpenApiSecurityRequirement
        {
          [new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            Name = "Bearer",
            In = ParameterLocation.Header,
          }] = new List<string>()
        }
      };
    }
  }
}
