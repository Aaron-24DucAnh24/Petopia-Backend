using PetAdoption.Data;
using PetAdoption.Data.Entities;
using PetAdoption.DataLayer.Interfaces;

namespace PetAdoption.DataLayer.Implementations
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