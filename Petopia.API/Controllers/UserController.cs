using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Email;
using Petopia.Business.Models.User;
using Petopia.Business.Utils;

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
      return ResponseUtils.OkResult(await _userService.GetCurrentUserAsync());
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> SendForgotPasswordMail([FromBody] string email)
    {
      MailDataModel mailMessage = await _emailService.CreateForgotPasswordMailDataAsync(email);
      _emailJobService.SendMail(mailMessage);
      return ResponseUtils.OkResult(true);
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> ResetPassword([FromQuery] ResetPasswordRequestModel request)
    {
      return ResponseUtils.OkResult(await _userService.ResetPasswordAsync(request));
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordRequestModel request)
    {
      return ResponseUtils.OkResult(await _userService.ChangePasswordAsync(request));
    }
  }
}