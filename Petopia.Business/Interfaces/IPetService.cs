﻿using Petopia.Business.Models.Common;
using Petopia.Business.Models.Pet;

namespace Petopia.Business.Interfaces
{
  public interface IPetService
  {
    public Task<CreatePetResponseModel> CreatePetAsync(CreatePetRequestModel model);
    public Task<PaginationResponseModel<PetResponseModel>> GetPetsAsync(PaginationRequestModel<PetFilterModel> model);
    public Task<PetDetailsResponseModel> GetPetDetailsAsync(Guid petId);
    public Task<UpdatePetResponseModel> UpdatePetAsync(UpdatePetRequestModel model);
    public Task<bool> DeletePetAsync(Guid petId);
  }
}