using Petopia.Business.Models.Payment;
using Petopia.Data.Entities;

namespace Petopia.Business.Interfaces
{
  public interface IPaymentService
  {
    public Task<string> GenerateTokenAsync();
    public Task<CreatePaymentResponseModel> CreatePaymentAsync(CreatePaymentRequestModel request);
    public Task<List<Advertisement>> GetAdvertisementAsync();
    public Task<List<SavedCardResponseModel>> GetSavedCardsAsync();
    public Task<SavedCardResponseModel> VaultCardAsync(VaultCardRequestModel request);
    public Task<bool> DeleteSavedCardAsync(string token);
  }
}