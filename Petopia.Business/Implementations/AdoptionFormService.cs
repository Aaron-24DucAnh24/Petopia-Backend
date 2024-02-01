using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Exception;
using Petopia.Data.Entities;
using Petopia.Data.Enums;
using Petopia.Business.Models.AdoptionForm;

namespace Petopia.Business.Implementations
{
    public class AdoptionFormService : BaseService, IAdoptionFormService
    {
        public AdoptionFormService(
                       IServiceProvider provider,
                       ILogger<AdoptionFormService> logger
                   ) : base(provider, logger)
        {
        }

        public async Task<AdoptionFormDataModel> GetAdoptionFormByIdAsync(Guid id)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.AdoptionFormId == id);
            if (adoptionForm == null)
            {
                throw new NotExistException();
            }
            else return Mapper.Map<AdoptionFormDataModel>(adoptionForm);
            
        }

        public async Task<AdoptionFormDataModel> GetAdoptionFormByPetIdAsync(Guid petId)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.PetId == petId);
            if (adoptionForm == null)
            {
                throw new NotExistException();
            }
            else return Mapper.Map<AdoptionFormDataModel>(adoptionForm);
            
        }

        public Task CreateAdoptionFormAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAdoptionFormAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task HandleAdoptionFormAsync(Guid id, AdoptStatus adoptStatus)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAdoptionFormAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}