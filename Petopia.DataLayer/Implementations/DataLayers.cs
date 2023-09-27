using Petopia.Data;
using Petopia.Data.Entities;
using Petopia.DataLayer.Interfaces;

namespace Petopia.DataLayer.Implementations
{
  public class UserDataLayer : BaseDataLayer<User>, IUserDataLayer
  {
    public UserDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }
  public class UserConnectionDataLayer : BaseDataLayer<UserConnection>, IUserConnectionDataLayer
  {
    public UserConnectionDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }
}