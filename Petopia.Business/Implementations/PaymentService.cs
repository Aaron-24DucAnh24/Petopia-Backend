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
using CreditCard = Braintree.CreditCard;

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

    public async Task<string> GenerateTokenAsync()
    {
      var userId = UserContext.Id;
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Id == userId);

      if (user != null && !string.IsNullOrEmpty(user.BraintreeCustomerId))
      {
        try
        {
          return await _gateway.ClientToken.GenerateAsync(new ClientTokenRequest
          {
            CustomerId = user.BraintreeCustomerId
          });
        }
        catch
        {
          // Customer no longer exists in Braintree — clear the stale ID and fall through
          var trackedUser = await UnitOfWork.Users.AsTracking().FirstAsync(x => x.Id == userId);
          trackedUser.BraintreeCustomerId = null;
          UnitOfWork.Users.Update(trackedUser);
          await UnitOfWork.SaveChangesAsync();
        }
      }

      try
      {
        return await _gateway.ClientToken.GenerateAsync(new ClientTokenRequest());
      }
      catch
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

      if (blog.AdvertisingDate.CompareTo(DateTimeOffset.UtcNow) >= 0)
      {
        throw new PaymentFailureException();
      }

      var transaction = new TransactionRequest()
      {
        Amount = advertisement.Price,
        Options = new TransactionOptionsRequest
        {
          SubmitForSettlement = true
        }
      };

      if (!string.IsNullOrEmpty(request.PaymentMethodToken))
      {
        var payingUserId = UserContext.Id;
        var payingUser = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Id == payingUserId);
        if (payingUser == null || string.IsNullOrEmpty(payingUser.BraintreeCustomerId))
          throw new PaymentFailureException();
        var payingCustomer = await _gateway.Customer.FindAsync(payingUser.BraintreeCustomerId);
        if (!payingCustomer.CreditCards.Any(c => c.Token == request.PaymentMethodToken))
          throw new PaymentFailureException();
        transaction.PaymentMethodToken = request.PaymentMethodToken;
      }
      else if (!string.IsNullOrEmpty(request.Nonce))
      {
        transaction.PaymentMethodNonce = request.Nonce;
      }
      else
      {
        throw new PaymentFailureException();
      }

      var result = await _gateway.Transaction.SaleAsync(transaction);
      if (!result.IsSuccess())
      {
        throw new PaymentFailureException();
      }

      var payment = await UnitOfWork.Payments.CreateAsync(new Payment()
      {
        Id = Guid.NewGuid(),
        BlogId = request.BlogId,
        AdvertisingDate = DateTimeOffset.UtcNow.AddDays(advertisement.MonthDuration * 30),
        IsCreatedAt = DateTimeOffset.UtcNow,
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

    public async Task<List<SavedCardResponseModel>> GetSavedCardsAsync()
    {
      var userId = UserContext.Id;
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Id == userId);

      if (user == null || string.IsNullOrEmpty(user.BraintreeCustomerId))
        return new List<SavedCardResponseModel>();

      try
      {
        var customer = await _gateway.Customer.FindAsync(user.BraintreeCustomerId);
        return customer.CreditCards
          .Select(MapCreditCard)
          .ToList();
      }
      catch
      {
        return new List<SavedCardResponseModel>();
      }
    }

    public async Task<SavedCardResponseModel> VaultCardAsync(VaultCardRequestModel request)
    {
      var customerId = await GetOrCreateBraintreeCustomerAsync();

      var customer = await _gateway.Customer.FindAsync(customerId);
      if (customer.CreditCards.Length >= 5)
        throw new MaxSavedCardsException();

      var paymentMethodRequest = new PaymentMethodRequest
      {
        CustomerId = customerId,
        PaymentMethodNonce = request.Nonce,
        Options = new PaymentMethodOptionsRequest { VerifyCard = true },
      };

      var result = await _gateway.PaymentMethod.CreateAsync(paymentMethodRequest);
      if (!result.IsSuccess())
        throw new VaultCardFailedException();

      var cc = result.Target as CreditCard;
      if (cc == null)
        throw new VaultCardFailedException();

      // Re-check after creation to handle concurrent vault requests that both passed the pre-check
      var updatedCustomer = await _gateway.Customer.FindAsync(customerId);
      if (updatedCustomer.CreditCards.Length > 5)
      {
        await _gateway.PaymentMethod.DeleteAsync(cc.Token);
        throw new MaxSavedCardsException();
      }

      return MapCreditCard(cc);
    }

    public async Task<bool> DeleteSavedCardAsync(string token)
    {
      var userId = UserContext.Id;
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Id == userId);

      if (user == null || string.IsNullOrEmpty(user.BraintreeCustomerId))
        throw new DeleteCardFailedException();

      try
      {
        var customer = await _gateway.Customer.FindAsync(user.BraintreeCustomerId);
        if (!customer.CreditCards.Any(c => c.Token == token))
          throw new DeleteCardFailedException();

        await _gateway.PaymentMethod.DeleteAsync(token);
        return true;
      }
      catch (DeleteCardFailedException)
      {
        throw;
      }
      catch
      {
        throw new DeleteCardFailedException();
      }
    }

    private async Task<string> GetOrCreateBraintreeCustomerAsync()
    {
      var userId = UserContext.Id;
      var user = await UnitOfWork.Users
        .AsTracking()
        .FirstAsync(x => x.Id == userId);

      if (!string.IsNullOrEmpty(user.BraintreeCustomerId))
        return user.BraintreeCustomerId;

      var customerRequest = new CustomerRequest
      {
        Email = HashUtils.DecryptString(user.Email),
      };

      var result = await _gateway.Customer.CreateAsync(customerRequest);
      if (!result.IsSuccess())
        throw new VaultCardFailedException();

      user.BraintreeCustomerId = result.Target.Id;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();

      return result.Target.Id;
    }

    private static SavedCardResponseModel MapCreditCard(CreditCard cc)
    {
      return new SavedCardResponseModel
      {
        Token = cc.Token,
        CardType = cc.CardType.ToString(),
        Last4 = cc.LastFour,
        ExpirationMonth = cc.ExpirationMonth,
        ExpirationYear = cc.ExpirationYear,
        CardholderName = cc.CardholderName ?? "",
        ImageUrl = cc.ImageUrl,
        IsDefault = cc.IsDefault ?? false,
      };
    }
  }
}