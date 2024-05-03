using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public PaymentController(
      IPaymentService paymentService
    )
    {
      _paymentService = paymentService;
    }

    [HttpGet("Token")]
    [OrganizationAuthorize]
    public async Task<ActionResult<string>> GetToken()
    {
      return ResponseUtils.OkResult(await _paymentService.GenerateTokenAsync());
    }

    [HttpPost]
    [OrganizationAuthorize]
    public async Task<ActionResult<CreatePaymentResponseModel>> CreatePayment([FromBody] CreatePaymentRequestModel request)
    {
      return ResponseUtils.OkResult(await _paymentService.CreatePaymentAsync(request));
    }

    [HttpGet("AdvertisementType")]
    [OrganizationAuthorize]
    public async Task<ActionResult<List<Advertisement>>> GetAdvertisement()
    {
      return ResponseUtils.OkResult(await _paymentService.GetAdvertisementAsync());
    }
  }
}