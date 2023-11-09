namespace Petopia.Business.Interfaces
{
  public interface IPaymentService
  {
    public Task<string> GenerateTokenAsync(string customerId = "");
  }
}