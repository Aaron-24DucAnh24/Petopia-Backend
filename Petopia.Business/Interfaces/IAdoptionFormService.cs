using Petopia.Data.Enums;

namespace Petopia.Business.Interfaces
{
    public interface IAdoptionFormService
    {
        public Task GetAdoptionFormByIdAsync(Guid id);

        public Task GetAdoptionFormByPetIdAsync(Guid petId);

        public Task CreateAdoptionFormAsync();

        public Task UpdateAdoptionFormAsync(Guid id);

        public Task HandleAdoptionFormAsync(Guid id, AdoptStatus adoptStatus);

        public Task DeleteAdoptionFormAsync(Guid id);
    }
}