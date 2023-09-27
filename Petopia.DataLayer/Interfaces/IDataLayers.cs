using Petopia.Data.Entities;

namespace Petopia.DataLayer.Interfaces
{
  public interface IUserConnectionDataLayer : IBaseDataLayer<UserConnection> { }
  public interface IUserDataLayer : IBaseDataLayer<User> { }
}