using Petopia.Business.Models.Payment;
using Petopia.Data.Entities;

namespace Petopia.Business.Interfaces
{
  public interface IPaymentService
  {
    public Task<string> GenerateTokenAsync(string customerId = "");
    public Task<CreatePaymentResponseModel> CreatePaymentAsync(CreatePaymentRequestModel request);
    public Task<List<Advertisement>> GetAdvertisementAsync();
  }
}