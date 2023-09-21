using PetAdoption.Data.Entities;

namespace PetAdoption.DataLayer.Interfaces
{
  public interface IUserConnectionDataLayer : IBaseDataLayer<UserConnection> { }
  public interface IUserDataLayer : IBaseDataLayer<User> { }
}