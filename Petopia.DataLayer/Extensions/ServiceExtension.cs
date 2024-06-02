using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petopia.Data;
using Petopia.Data.Entities;
using Petopia.DataLayer.Implementations;
using Petopia.DataLayer.Interfaces;

namespace Petopia.DataLayer.Extensions
{
  public static class ServiceExtension
  {
    public static void AddApplicationDbContext(
      this IServiceCollection services,
      IConfiguration configuration,
      string ConnectionStringName)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(configuration.GetConnectionString(ConnectionStringName));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      }, ServiceLifetime.Scoped);
    }

    public static void AddDataLayerServices(this IServiceCollection services)
    {
      services.AddScoped<IUserDataLayer, UserDataLayer>();
      services.AddScoped<IUserConnectionDataLayer, UserConnectionDataLayer>();
      services.AddScoped<ISyncDataCollectionDataLayer, SyncDataCollectionDataLayer>();
      services.AddScoped<IUserIndividualAttributesDataLayer, UserIndividualAttributesDataLayer>();
      services.AddScoped<IUserOrganizationAttributesDataLayer, UserOrganizationAttributesDataLayer>();
      services.AddScoped<IEmailTemplateDataLayer, EmailTemplateDataLayer>();
      services.AddScoped<IPetDataLayer, PetDataLayer>();
      services.AddScoped<IMediaDataLayer, MediaDataLayer>();
      services.AddScoped<IProvinceDataLayer, ProvinceDataLayer>();
      services.AddScoped<IDistrictDataLayer, DistrictDataLayer>();
      services.AddScoped<IWardDataLayer, WardDataLayer>();
      services.AddScoped<IAdoptionFormDataLayer, AdoptionFormDataLayer>();
      services.AddScoped<INotificationDataLayer, NotificationFormDataLayer>();
      services.AddScoped<IUpgradeFormDataLayer, UpgradeFormDataLayer>();
      services.AddScoped<IBlogDataLayer, BlogDataLayer>();
      services.AddScoped<IPostDataLayer, PostDataLayer>();
      services.AddScoped<ICommentDataLayer, CommentDataLayer>();
      services.AddScoped<ILikeDataLayer, LikeDataLayer>();
      services.AddScoped<IPetBreedDataLayer, PetBreedDataLayer>();
      services.AddScoped<IAdminFormDataLayer, AdminFormDataLayer>();
      services.AddScoped<IPaymentDataLayer, PaymentDataLayer>();
      services.AddScoped<IAdvertisementDataLayer, AdvertisementDataLayer>();
      services.AddScoped<IReportDataLayer, ReportDataLayer>();
      services.AddScoped<IVaccineDataLayer, VaccineDataLayer>();
      services.AddScoped<IPetVaccineDataLayer, PetVaccineDataLayer>();
    }
  }
}