using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
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
    private readonly IAuthService _authService;
    private readonly IEmailJobService _emailJobService;

    public UserController(
      IUserService userService,
      IEmailService emailService,
      IAuthService authService,
      IEmailJobService emailJobService
    )
    {
      _userService = userService;
      _emailService = emailService;
      _authService = authService;
      _emailJobService = emailJobService;
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> SendForgotPasswordEmail([FromBody] string email)
    {
      var mailMessage = await _emailService.CreateForgotPasswordMailDataAsync(email);
      _emailJobService.SendMail(mailMessage);
      return Ok(true);
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> ResetPassword([FromBody] ResetPasswordRequestModel request)
    {
      await _userService.ResetPasswordAsync(request);
      return Ok(true);
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordRequestModel request)
    {
      await _userService.ChangePasswordAsync(request);
      return Ok(true);
    }
  }
}