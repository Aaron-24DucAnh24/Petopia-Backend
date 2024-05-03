using Braintree;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Payment;
using Petopia.Business.Utils;
using Petopia.Data.Entities;

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

    public async Task<CreatePaymentResponseModel> CreatePaymentAsync(CreatePaymentRequestModel request)
    {
      Advertisement advertisement = await UnitOfWork.Advertisements
        .FirstAsync(x => x.Id == request.AdvertisementId);
      Blog blog = await UnitOfWork.Blogs
        .Include(x => x.User)
        .FirstAsync(x => x.Id == request.BlogId);

      TransactionRequest transaction = new()
      {
        Amount = advertisement.Price,
        PaymentMethodNonce = request.Nonce,
        Options = new TransactionOptionsRequest
        {
          SubmitForSettlement = true
        }
      };

      Result<Transaction> result = await _gateway.Transaction.SaleAsync(transaction);
      if (!result.IsSuccess())
      {
        throw new PaymentFailureException();
      }

      Payment payment = await UnitOfWork.Payments.CreateAsync(new Payment()
      {
        Id = Guid.NewGuid(),
        BlogId = request.BlogId,
        AdvertisingDate = DateTimeOffset.Now.AddDays(advertisement.MonthDuration * 12),
        IsCreatedAt = DateTimeOffset.Now,
        Amount = advertisement.Price,
      });

      return new CreatePaymentResponseModel()
      {
        PaymentId = payment.Id,
        Price = payment.Amount,
        IsCreatedAt = payment.IsCreatedAt,
        AdvertisingDate = payment.AdvertisingDate,
        UserEmail = HashUtils.DecryptString(blog.User.Email),
      };
    }

    public async Task<List<Advertisement>> GetAdvertisementAsync()
    {
      return await UnitOfWork.Advertisements.ToListAsync();
    }
  }
}