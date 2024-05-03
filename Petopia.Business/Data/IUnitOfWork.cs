using Petopia.DataLayer.Interfaces;

namespace Petopia.Business.Data
{
  public interface IUnitOfWork
  {
    int SaveChange();
    ValueTask<int> SaveChangesAsync();
    IUserDataLayer Users { get; }
    IUserConnectionDataLayer UserConnections { get; }
    ISyncDataCollectionDataLayer SyncDataCollections { get; }
    IUserIndividualAttributesDataLayer UserIndividualAttributes { get; }
    IUserOrganizationAttributesDataLayer UserOrganizationAttributes { get; }
    IEmailTemplateDataLayer EmailTemplates { get; }
    IPetDataLayer Pets { get; }
    IMediaDataLayer Medias { get; }
    IProvinceDataLayer Provinces { get; }
    IDistrictDataLayer Districts { get; }
    IWardDataLayer Wards { get; }
    IAdoptionFormDataLayer AdoptionForms { get; }
    INotificationDataLayer Notifications { get; }
    IUpgradeFormDataLayer UpgradeForms { get; }
    IBlogDataLayer Blogs { get; }
    IPostDataLayer Posts { get; }
    ICommentDataLayer Comments { get; }
    ILikeDataLayer Likes { get; }
    IPetBreedDataLayer PetBreeds { get; }
    IAdminFormDataLayer AdminForms { get; }
    IPaymentDataLayer Payments { get; }
    IAdvertisementDataLayer Advertisements { get; }
  }
}