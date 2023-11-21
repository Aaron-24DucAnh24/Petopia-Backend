using Petopia.Data.Entities;

namespace Petopia.DataLayer.Interfaces
{
  public interface IUserConnectionDataLayer : IBaseDataLayer<UserConnection> { }
  public interface IUserDataLayer : IBaseDataLayer<User> { }
  public interface ISyncDataCollectionDataLayer : IBaseDataLayer<SyncDataCollection> { }
  public interface IUserIndividualAttributesDataLayer : IBaseDataLayer<UserIndividualAttributes> { }
  public interface IUserOrganizationAttributesDataLayer : IBaseDataLayer<UserOrganizationAttributes> { }
  public interface IEmailDataLayer : IBaseDataLayer<Email> { }
}