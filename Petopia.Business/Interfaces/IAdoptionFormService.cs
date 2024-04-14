using Petopia.Business.Models.Adoption;
using Petopia.Data.Enums;

namespace Petopia.Business.Interfaces
{
	public interface IAdoptionFormService
	{
		public Task<bool> PreCheckAsync(Guid petId);
		public Task<bool> CreateAdoptionFormAsync(CreateAdoptionRequestModel request);
		public Task<bool> ActOnAdoptionFormAsync(Guid formId, AdoptStatus status);
		public Task<bool> DeleteAdoptionFormAsync(Guid formId);
		public Task<List<AdoptionFormResponseModel>> GetAdoptionFormsIncomingAsync();
		public Task<List<AdoptionFormResponseModel>> GetAdoptionFormsByUserIdAsync();
		public Task<DetailAdoptionFormResponseModel> GetAdoptionFormAsync(Guid formId);
	}
}