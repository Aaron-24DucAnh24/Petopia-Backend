using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.AdoptionForm;
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

        public async Task<Guid> CreateAdoptionFormAsync()
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.CreateAsync(new AdoptionForm()
            {
                AdoptionFormId = Guid.NewGuid(),

                //PetId = getPetId
                //UserId = UserContext.Id,
                /*     Fname = UserContext.FName,
                     Lname = UserContext.LName,
                     ... */
            });

            await UnitOfWork.SaveChangesAsync();

            return adoptionForm.AdoptionFormId;
        }

        public async Task UpdateAdoptionFormAsync(AdoptionFormDataModel data)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.AdoptionFormId == data.AdoptionFormId);
            if (adoptionForm == null)
            {
                throw new NotExistException();
            }
            else
            {
                adoptionForm.AdoptStatus = AdoptStatus.Pending;
                /*                adoptionForm.FName = data.FName;
                                adoptionForm.LName = data.LName;
                                adoptionForm.Age = data.Age;
                                adoptionForm.Email = data.Email;
                                adoptionForm.PhoneNum = data.PhoneNum;
                                adoptionForm.Adr = data.Adr;
                                adoptionForm.AdrCity = data.AdrCity;
                                adoptionForm.AdrDistrict = data.AdrDistrict;*/
                adoptionForm.IsPetOwner = data.IsPetOwner;
                adoptionForm.HouseType = data.HouseType;
                adoptionForm.TakePetDuration = data.TakePetDuration;

                await UnitOfWork.SaveChangesAsync();
            }
        }

        public async Task HandleAdoptionFormAsync(Guid id, AdoptStatus adoptStatus)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.AdoptionFormId == id);
            if (adoptionForm == null)
            {
                throw new NotExistException();
            }
            else
            {
                adoptionForm.AdoptStatus = adoptStatus;
                //notify user
                //NotifyUser(adoptionForm.UserId, adoptStatus);
                await UnitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAdoptionFormAsync(Guid id)
        {
            AdoptionForm adoptionForm = await UnitOfWork.AdoptionForms.FirstAsync(x => x.AdoptionFormId == id);
            if (adoptionForm != null)
            {
                UnitOfWork.AdoptionForms.Delete(adoptionForm);
                await UnitOfWork.SaveChangesAsync();
            }
        }
    }
}