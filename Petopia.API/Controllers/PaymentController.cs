using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Filters;
using Petopia.Business.Interfaces;

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

    [HttpGet("GetToken")]
    [OrganizationAuthorize]
    public async Task<ActionResult<string>> GetToken()
    {
      return Ok(await _paymentService.GenerateTokenAsync());
    }
  }
}