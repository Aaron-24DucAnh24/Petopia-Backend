using Braintree;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Exceptions;

namespace Petopia.Business.Implementations
{
  public class PaymentService : BaseService, IPaymentService
  {
    private readonly IBraintreeGateway _gateway;
    public PaymentService(
      IServiceProvider provider,
      ILogger<PaymentService> logger,
      IBraintreeGateway gateway
    ) : base(provider, logger)
    {
      _gateway = gateway;
    }

    public async Task<string> GenerateTokenAsync(string customerId = "")
    {
      try
      {
        ClientTokenRequest clientTokenRequest = new();
        if (!string.IsNullOrEmpty(customerId))
        {
          clientTokenRequest.CustomerId = customerId;
        }
        return await _gateway.ClientToken.GenerateAsync(clientTokenRequest);
      }
      catch (Exception)
      {
        throw new PaymentTokenException();
      }
    }
  }
}