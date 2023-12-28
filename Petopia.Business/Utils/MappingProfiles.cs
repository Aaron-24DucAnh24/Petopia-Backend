using AutoMapper;
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

      CreateMap<CreatePetRequestModel, CreatePetResponseModel>();

      CreateMap<UpdatePetRequestModel, UpdatePetResponseModel>();
    }
  }
}