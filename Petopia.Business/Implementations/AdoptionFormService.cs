using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Exception;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

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

        public async Task GetAdoptionFormByIdAsync(Guid id)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.AdoptionFormId == id);
            if (adoptionForm == null)
            {
                throw new NotExistException();
            }
            else return Mapper.Map<AdoptionFormResponseModel>(adoptionForm);
            
        }

        public Task GetAdoptionFormByPetIdAsync(Guid petId)
        {
            throw new NotImplementedException();
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