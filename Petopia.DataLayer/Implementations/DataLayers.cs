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

	public class BlogDataLayer : BaseDataLayer<Blog>, IBlogDataLayer
	{
		public BlogDataLayer(ApplicationDbContext context) : base(context)
		{
		}
	}
}