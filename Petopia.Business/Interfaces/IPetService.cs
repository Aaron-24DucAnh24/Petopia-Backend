using Petopia.Business.Models.Common;
using Petopia.Business.Models.Pet;
using Petopia.Data.Enums;

namespace Petopia.Business.Interfaces
{
  public interface IPetService
  {
    public Task<CreatePetResponseModel> CreatePetAsync(CreatePetRequestModel model);
    public Task<PaginationResponseModel<PetResponseModel>> GetPetsAsync(PaginationRequestModel<PetFilterModel> model);
		public Task<PaginationResponseModel<PetResponseModel>> GetPetsByUserId(PaginationRequestModel<Guid> model);
		public Task<PetDetailsResponseModel> GetPetDetailsAsync(Guid petId);
    public Task<UpdatePetResponseModel> UpdatePetAsync(UpdatePetRequestModel model);
    public Task<bool> DeletePetAsync(Guid petId);
    public Task<List<string>> GetBreedsAsync(PetSpecies species);
    public Task<List<string>> GetAvailableBreedsAsync(PetSpecies species);
	}
}
