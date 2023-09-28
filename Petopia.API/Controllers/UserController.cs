using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.User;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/User")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IValidationService _validationService;

    public UserController(
      IUserService userService,
      IEmailService emailService,
      IValidationService validationService
    )
    {
      _userService = userService;
      _emailService = emailService;
      _validationService = validationService;
    }

    [HttpGet("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult> SendForgotPasswordEmail([FromQuery] string email)
    {
      await _emailService.SendForgotPasswordEmailAsync(email);
      return Ok(true);
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
      if (!await _validationService.ValidateAsync(request, ModelState))
      {
        return BadRequest(ModelState);
      }
      await _userService.ResetPasswordAsync(request);
      return Ok(true);
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
      if (!await _validationService.ValidateAsync(request, ModelState))
      {
        return BadRequest(ModelState);
      }
      await _userService.ChangePasswordAsync(request);
      return Ok(true);
    }
  }
}