using Braintree;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Payment;
using Petopia.Business.Utils;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
{
  public class PaymentService : BaseService, IPaymentService
  {
    private readonly IBraintreeGateway _gateway;
    private readonly ISearchEngineService _searchEngineService;

    public PaymentService(
      IServiceProvider provider,
      ILogger<PaymentService> logger,
      IBraintreeGateway gateway
    ) : base(provider, logger)
    {
      _gateway = gateway;
      _searchEngineService = provider.GetRequiredService<ISearchEngineService>();
    }

    public async Task<string> GenerateTokenAsync(string customerId = "")
    {
      try
      {
        var clientTokenRequest = new ClientTokenRequest();
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
      var advertisement = await UnitOfWork.Advertisements
        .FirstAsync(x => x.Id == request.AdvertisementId);
      var blog = await UnitOfWork.Blogs
        .AsTracking()
        .Include(x => x.User).ThenInclude(x => x.UserOrganizationAttributes)
        .Include(x => x.User).ThenInclude(x => x.UserIndividualAttributes)
        .FirstAsync(x => x.Id == request.BlogId);

      if (blog.AdvertisingDate.CompareTo(DateTimeOffset.Now) >= 0)
      {
        throw new PaymentFailureException();
      }

      var transaction = new TransactionRequest()
      {
        Amount = advertisement.Price,
        PaymentMethodNonce = request.Nonce,
        Options = new TransactionOptionsRequest
        {
          SubmitForSettlement = true
        }
      };

      var result = await _gateway.Transaction.SaleAsync(transaction);
      if (!result.IsSuccess())
      {
        throw new PaymentFailureException();
      }

      var payment = await UnitOfWork.Payments.CreateAsync(new Payment()
      {
        Id = Guid.NewGuid(),
        BlogId = request.BlogId,
        AdvertisingDate = DateTimeOffset.Now.AddDays(advertisement.MonthDuration * 30),
        IsCreatedAt = DateTimeOffset.Now,
        Amount = advertisement.Price,
        LinkedPaymentId = result.Target.Id,
      });

      blog.AdvertisingDate = payment.AdvertisingDate;
      UnitOfWork.Blogs.Update(blog);
      await UnitOfWork.SaveChangesAsync();
      await _searchEngineService.InsertUpdateAsync(Constants.MEILISEARCH_INDEX_BLOG, Mapper.Map<BlogSearchModel>(blog));

      return new CreatePaymentResponseModel()
      {
        PaymentId = payment.Id,
        Price = payment.Amount,
        IsCreatedAt = payment.IsCreatedAt,
        AdvertisingDate = payment.AdvertisingDate,
        UserEmail = HashUtils.DecryptString(blog.User.Email),
        Description = advertisement.Description,
      };
    }

    public async Task<List<Advertisement>> GetAdvertisementAsync()
    {
      var result = await UnitOfWork.Advertisements.ToListAsync();
      return result.OrderBy(x => x.MonthDuration).ToList();
    }
  }
}