using PetAdoption.DataLayer.Interfaces;

namespace PetAdoption.Business.Data
{
  public interface IUnitOfWork
  {
    int SaveChange();
    ValueTask<int> SaveChangesAsync();
    IUserDataLayer Users { get; }
    IUserConnectionDataLayer UserConnections { get; }
  }
}