using Petopia.Data.Entities;

namespace Petopia.DataLayer.Interfaces
{
  public interface IUserConnectionDataLayer : IBaseDataLayer<UserConnection> { }
  public interface IUserDataLayer : IBaseDataLayer<User> { }
  public interface ISyncDataCollectionDataLayer : IBaseDataLayer<SyncDataCollection> { }
  public interface IUserIndividualAttributesDataLayer : IBaseDataLayer<UserIndividualAttributes> { }
  public interface IUserOrganizationAttributesDataLayer : IBaseDataLayer<UserOrganizationAttributes> { }
  public interface IEmailTemplateDataLayer : IBaseDataLayer<EmailTemplate> { }
  public interface IPetDataLayer : IBaseDataLayer<Pet> { }
  public interface IMediaDataLayer : IBaseDataLayer<Media> { }
  public interface IProvinceDataLayer : IBaseDataLayer<Province> { }
  public interface IDistrictDataLayer : IBaseDataLayer<District> { }
  public interface IWardDataLayer : IBaseDataLayer<Ward> { }
  public interface IAdoptionFormDataLayer : IBaseDataLayer<AdoptionForm> { }
  public interface INotificationDataLayer : IBaseDataLayer<Notification> { }
  public interface IUpgradeFormDataLayer : IBaseDataLayer<UpgradeForm> { }
  public interface IBlogDataLayer : IBaseDataLayer<Blog> { }
  public interface IPostDataLayer : IBaseDataLayer<Post> { }
  public interface ICommentDataLayer : IBaseDataLayer<Comment> { }
  public interface ILikeDataLayer : IBaseDataLayer<Like> { }
  public interface IPetBreedDataLayer : IBaseDataLayer<PetBreed> { }
}