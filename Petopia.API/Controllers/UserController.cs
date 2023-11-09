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
    private readonly IEmailJobService _emailJobService;

    public UserController(
      IUserService userService,
      IEmailService emailService,
      IEmailJobService emailJobService
    )
    {
      _userService = userService;
      _emailService = emailService;
      _emailJobService = emailJobService;
    }

    [HttpGet("CurrentUser")]
    [Authorize]
    public async Task<ActionResult<CurrentUserResponseModel>> GetCurrentUser()
    {
      return Ok(await _userService.GetCurrentUserAsync());
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> SendForgotPasswordMail([FromBody] string email)
    {
      var mailMessage = await _emailService.CreateForgotPasswordMailDataAsync(email);
      _emailJobService.SendMail(mailMessage);
      return Ok(true);
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> ResetPassword([FromBody] ResetPasswordRequestModel request)
    {
      return Ok(await _userService.ResetPasswordAsync(request));
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordRequestModel request)
    {
      return Ok(await _userService.ChangePasswordAsync(request));
    }
  }
}