using Petopia.Business.Models.AdoptionForm;
using Petopia.Data.Enums;

namespace Petopia.Business.Interfaces
{
    public interface IAdoptionFormService
    {
        public Task<AdoptionFormDataModel> GetAdoptionFormByIdAsync(Guid id);

        public Task<AdoptionFormDataModel> GetAdoptionFormByPetIdAsync(Guid petId);

        public Task CreateAdoptionFormAsync();

        public Task UpdateAdoptionFormAsync(Guid id, AdoptionFormDataModel data);

        public Task HandleAdoptionFormAsync(Guid id, AdoptStatus adoptStatus);

        public Task DeleteAdoptionFormAsync(Guid id);
    }
}