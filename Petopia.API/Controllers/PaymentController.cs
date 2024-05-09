using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Email;
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
    private readonly IEmailService _emailService;

    public PaymentController(
      IPaymentService paymentService,
      IEmailJobService emailJobService,
      IEmailService emailService
    )
    {
      _paymentService = paymentService;
      _emailJobService = emailJobService;
      _emailService = emailService;
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
      CreatePaymentResponseModel paymentResponse = await _paymentService.CreatePaymentAsync(request);
      MailDataModel mailData = await _emailService.CreateInvoiceMailDataAsync(paymentResponse);
      _emailJobService.SendMail(mailData);
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