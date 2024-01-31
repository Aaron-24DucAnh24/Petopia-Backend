using Microsoft.Extensions.DependencyInjection;
using Petopia.Data;
using Petopia.DataLayer.Interfaces;

namespace Petopia.Business.Data
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

    public int SaveChange()
    {
      return _dbContext.SaveChanges();
    }

    public async ValueTask<int> SaveChangesAsync()
    {
      return await _dbContext.SaveChangesAsync();
    }

    public IUserDataLayer Users => _serviceProvider.GetRequiredService<IUserDataLayer>();
    public IUserConnectionDataLayer UserConnections => _serviceProvider.GetRequiredService<IUserConnectionDataLayer>();
    public ISyncDataCollectionDataLayer SyncDataCollections => _serviceProvider.GetRequiredService<ISyncDataCollectionDataLayer>();
    public IUserIndividualAttributesDataLayer UserIndividualAttributes => _serviceProvider.GetRequiredService<IUserIndividualAttributesDataLayer>();
    public IUserOrganizationAttributesDataLayer UserOrganizationAttributes => _serviceProvider.GetRequiredService<IUserOrganizationAttributesDataLayer>();
    public IEmailDataLayer Emails => _serviceProvider.GetRequiredService<IEmailDataLayer>();
    public IAdoptionFormDataLayer AdoptionForms => _serviceProvider.GetRequiredService<IAdoptionFormDataLayer>();
  }
}