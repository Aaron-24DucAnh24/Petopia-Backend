using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Petopia.Data.Enums;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Petopia.Business.Filters
{
  public class AccessRoleFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      var apiAttributes = context.ApiDescription.CustomAttributes();
      var attribute = apiAttributes.FirstOrDefault(a => a is AuthorizeAttribute);
      if (attribute != null)
      {
        var summary = string.Empty;
        switch (attribute)
        {
          case AdminAuthorize _:
            summary = UserRole.SystemAdmin.ToString();
            break;
          case OrganizationAuthorize _:
            summary = UserRole.Organization.ToString();
            break;
        }
        operation.Summary = string.IsNullOrEmpty(summary) ? string.Empty : $"{summary} API";
      }
    }
  }
}
