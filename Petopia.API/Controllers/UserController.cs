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
    public UserController(
      IUserService userService,
      IEmailService emailService
    )
    {
      _userService = userService;
      _emailService = emailService;
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
      await _userService.ResetPasswordAsync(request);
      return Ok(true);
    }
  }
}