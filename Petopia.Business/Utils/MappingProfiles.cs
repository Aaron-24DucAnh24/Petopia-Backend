using AutoMapper;
using Petopia.Business.Models.Admin;
using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Comment;
using Petopia.Business.Models.Location;
using Petopia.Business.Models.Notification;
using Petopia.Business.Models.Pet;
using Petopia.Business.Models.Post;
using Petopia.Business.Models.User;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Utils
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<User, CurrentIndividualResponseModel>()
        .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.UserIndividualAttributes));
      CreateMap<User, CurrentOrganizationResponseModel>()
        .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.UserOrganizationAttributes));
      CreateMap<UserIndividualAttributes, CurrentIndividualAttributesResponseModel>();
      CreateMap<UserOrganizationAttributes, CurrentOrganizationAttributesResponseModel>();
      CreateMap<User, CurrentUserCoreResponseModel>();
      CreateMap<User, ManagementUserResponseModel>()
        .ForMember(dest => dest.OrganizationType, opt => opt.MapFrom(src => src.UserOrganizationAttributes.Type))
        .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.UserOrganizationAttributes.OrganizationName))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserIndividualAttributes.FirstName + " " + src.UserIndividualAttributes.LastName))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => HashUtils.DecryptString(src.Email)));

      CreateMap<CreatePetRequestModel, CreatePetResponseModel>();
      CreateMap<UpdatePetRequestModel, UpdatePetResponseModel>();
      CreateMap<Pet, PetResponseModel>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images[0].Url))
        .ForMember(dest => dest.IsOrgOwned, opt => opt.MapFrom(src => src.Owner.Role != UserRole.StandardUser));
      CreateMap<Pet, PetDetailsResponseModel>()
        .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()));
      CreateMap<Pet, ManagementPetResponseModel>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images[0].Url))
        .ForMember(dest => dest.OwnerImage, opt => opt.MapFrom(src => src.Owner.Image));
      CreateMap<Pet, PetSearchModel>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images[0].Url))
        .ForMember(dest => dest.IsOrgOwned, opt => opt.MapFrom(src => src.Owner.Role != UserRole.StandardUser));
      CreateMap<PetSearchModel, PetResponseModel>();

      CreateMap<Vaccine, VaccineResponseModel>();

      CreateMap<Province, LocationResponseModel>();
      CreateMap<District, LocationResponseModel>();
      CreateMap<Ward, LocationResponseModel>();

      CreateMap<Notification, NotificationResponseModel>();

      CreateMap<Blog, BlogDetailResponseModel>()
        .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Image))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserOrganizationAttributes.OrganizationName))
        .ForMember(dest => dest.IsAdvertised, opt => opt.MapFrom(src => src.AdvertisingDate.CompareTo(DateTimeOffset.Now) >= 0));
      CreateMap<Blog, BlogResponseModel>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserOrganizationAttributes.OrganizationName));
      CreateMap<Blog, ManagementBlogResponseModel>()
        .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Image));
      CreateMap<Blog, BlogSearchModel>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserOrganizationAttributes.OrganizationName));
      CreateMap<BlogSearchModel, BlogResponseModel>();

      CreateMap<Post, PostResponseModel>()
        .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()))
        .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Image))
        .ForMember(
          dest => dest.UserName,
          opt => opt.MapFrom(src => $"{src.User.UserIndividualAttributes.FirstName} {src.User.UserIndividualAttributes.LastName}"))
        .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));

      CreateMap<Comment, CommentResponseModel>();

      CreateMap<UpgradeForm, ManagementUpgradeResponseModel>()
        .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Image));
    }
  }
}