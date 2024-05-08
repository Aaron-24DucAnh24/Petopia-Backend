using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Payment;
using Petopia.Business.Utils;
using Petopia.Data.Entities;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Payment")]
  public class PaymentController : ControllerBase
  {
    private readonly IPaymentService _paymentService;
    private readonly IEmailJobService _emailJobService;

    public PaymentController(
      IPaymentService paymentService,
      IEmailJobService emailJobService
    )
    {
      _paymentService = paymentService;
      _emailJobService = emailJobService;
    }

    [HttpGet("Token")]
    [OrganizationAuthorize]
    public async Task<ActionResult<string>> GetToken()
    {
      return ResponseUtils.OkResult(await _paymentService.GenerateTokenAsync());
    }

    [HttpPost]
    [OrganizationAuthorize]
    public async Task<ActionResult<bool>> CreatePayment([FromBody] CreatePaymentRequestModel request)
    {
      CreatePaymentResponseModel result = await _paymentService.CreatePaymentAsync(request);

      return ResponseUtils.OkResult(true);
    }

    [HttpGet("AdvertisementType")]
    [OrganizationAuthorize]
    public async Task<ActionResult<List<Advertisement>>> GetAdvertisement()
    {
      return ResponseUtils.OkResult(await _paymentService.GetAdvertisementAsync());
    }
  }
}