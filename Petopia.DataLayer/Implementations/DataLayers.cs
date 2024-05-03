using Petopia.Data;
using Petopia.Data.Entities;
using Petopia.DataLayer.Interfaces;

namespace Petopia.DataLayer.Implementations
{
  public class UserDataLayer : BaseDataLayer<User>, IUserDataLayer
  {
    public UserDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class UserConnectionDataLayer : BaseDataLayer<UserConnection>, IUserConnectionDataLayer
  {
    public UserConnectionDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class SyncDataCollectionDataLayer : BaseDataLayer<SyncDataCollection>, ISyncDataCollectionDataLayer
  {
    public SyncDataCollectionDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class UserIndividualAttributesDataLayer : BaseDataLayer<UserIndividualAttributes>, IUserIndividualAttributesDataLayer
  {
    public UserIndividualAttributesDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class UserOrganizationAttributesDataLayer : BaseDataLayer<UserOrganizationAttributes>, IUserOrganizationAttributesDataLayer
  {
    public UserOrganizationAttributesDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class EmailTemplateDataLayer : BaseDataLayer<EmailTemplate>, IEmailTemplateDataLayer
  {
    public EmailTemplateDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class PetDataLayer : BaseDataLayer<Pet>, IPetDataLayer
  {
    public PetDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class MediaDataLayer : BaseDataLayer<Media>, IMediaDataLayer
  {
    public MediaDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class ProvinceDataLayer : BaseDataLayer<Province>, IProvinceDataLayer
  {
    public ProvinceDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class DistrictDataLayer : BaseDataLayer<District>, IDistrictDataLayer
  {
    public DistrictDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class WardDataLayer : BaseDataLayer<Ward>, IWardDataLayer
  {
    public WardDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class AdoptionFormDataLayer : BaseDataLayer<AdoptionForm>, IAdoptionFormDataLayer
  {
    public AdoptionFormDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class NotificationFormDataLayer : BaseDataLayer<Notification>, INotificationDataLayer
  {
    public NotificationFormDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class UpgradeFormDataLayer : BaseDataLayer<UpgradeForm>, IUpgradeFormDataLayer
  {
    public UpgradeFormDataLayer(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
  }

  public class BlogDataLayer : BaseDataLayer<Blog>, IBlogDataLayer
  {
    public BlogDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class PostDataLayer : BaseDataLayer<Post>, IPostDataLayer
  {
    public PostDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class CommentDataLayer : BaseDataLayer<Comment>, ICommentDataLayer
  {
    public CommentDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class LikeDataLayer : BaseDataLayer<Like>, ILikeDataLayer
  {
    public LikeDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class PetBreedDataLayer : BaseDataLayer<PetBreed>, IPetBreedDataLayer
  {
    public PetBreedDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class AdminFormDataLayer : BaseDataLayer<AdminForm>, IAdminFormDataLayer
  {
    public AdminFormDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class PaymentDataLayer : BaseDataLayer<Payment>, IPaymentDataLayer
  {
    public PaymentDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }

  public class AdvertisementDataLayer : BaseDataLayer<Advertisement>, IAdvertisementDataLayer
  {
    public AdvertisementDataLayer(ApplicationDbContext context) : base(context)
    {
    }
  }
}