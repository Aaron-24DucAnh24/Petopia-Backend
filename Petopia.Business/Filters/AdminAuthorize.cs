using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Petopia.Business.Contexts;
using Petopia.Business.Models.Exceptions;
using Petopia.Data.Enums;

namespace Petopia.Business.Filters
{
  public class AdminAuthorize : AuthorizeAttribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      var userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
      if (userContext.Role != UserRole.SystemAdmin)
      {
        throw new ForbiddenAccessException();
      }
    }
  }
}