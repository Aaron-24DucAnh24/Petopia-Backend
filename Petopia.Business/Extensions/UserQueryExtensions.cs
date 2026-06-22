using Microsoft.EntityFrameworkCore;
using Petopia.Data.Entities;
using Petopia.DataLayer.Interfaces;

namespace Petopia.Business.Extensions
{
  public static class UserQueryExtensions
  {
    // Called directly on UnitOfWork.Users (IBaseDataLayer<User>)
    public static IQueryable<User> WithAttributes(this IBaseDataLayer<User> dataLayer)
      => dataLayer
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes);

    // Called after .AsTracking() which returns IQueryable<User>
    public static IQueryable<User> WithAttributes(this IQueryable<User> query)
      => query
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes);
  }
}
