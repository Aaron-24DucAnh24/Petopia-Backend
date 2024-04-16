using AutoMapper;
using Petopia.Business.Models.Adoption;
using Petopia.Business.Models.Location;
using Petopia.Business.Models.Notification;
using Petopia.Business.Models.Pet;
using Petopia.Business.Models.User;
using Petopia.Data.Entities;

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

      CreateMap<CreatePetRequestModel, CreatePetResponseModel>();
      CreateMap<UpdatePetRequestModel, UpdatePetResponseModel>();
      CreateMap<Pet, PetResponseModel>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images[0].Url));
      CreateMap<Pet, PetDetailsResponseModel>()
        .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.Url).ToList()));

      CreateMap<Province, LocationResponseModel>();
      CreateMap<District, LocationResponseModel>();
      CreateMap<Ward, LocationResponseModel>();

      CreateMap<AdoptionForm, DetailAdoptionFormResponseModel>()
        .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Adopter.Address));

      CreateMap<Notification, NotificationResponseModel>();
		}
  }
}