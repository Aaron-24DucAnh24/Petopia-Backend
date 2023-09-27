using Petopia.DataLayer.Interfaces;

namespace Petopia.Business.Data
{
  public interface IUnitOfWork
  {
    int SaveChange();
    ValueTask<int> SaveChangesAsync();
    IUserDataLayer Users { get; }
    IUserConnectionDataLayer UserConnections { get; }
  }
}