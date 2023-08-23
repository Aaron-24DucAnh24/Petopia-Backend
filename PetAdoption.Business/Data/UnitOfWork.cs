using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Data;
using PetAdoption.DataLayer.Interfaces;

namespace PetAdoption.Business.Data
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
      _dbContext = dbContext;
      _serviceProvider = serviceProvider;
    }

    public IUserDataLayer Users => _serviceProvider.GetRequiredService<IUserDataLayer>();
    public IUserConnectionDataLayer UserConnections => _serviceProvider.GetRequiredService<IUserConnectionDataLayer>();

    public int SaveChange()
    {
      return _dbContext.SaveChanges();
    }

    public async ValueTask<int> SaveChangesAsync()
    {
      return await _dbContext.SaveChangesAsync();
    }
  }
}