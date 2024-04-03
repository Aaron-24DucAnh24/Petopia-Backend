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

		[HttpGet("OtherUser")]
		[AllowAnonymous]
		public async Task<ActionResult<CurrentUserResponseModel>> GetOtherUser([FromQuery] string userId)
		{
			return ResponseUtils.OkResult(await _userService.GetOtherUserAsync(userId));
		}

		[HttpGet("CurrentUserCore")]
		[Authorize]
		public async Task<ActionResult<CurrentUserCoreResponseModel>> GetCurrentUserCore()
		{
			return ResponseUtils.OkResult(await _userService.GetCurrentUserCoreAsync());
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
		public async Task<ActionResult<bool>> ResetPassword([FromBody] ResetPasswordRequestModel request)
		{
			return ResponseUtils.OkResult(await _userService.ResetPasswordAsync(request));
		}

		[HttpPost("ChangePassword")]
		[Authorize]
		public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordRequestModel request)
		{
			return ResponseUtils.OkResult(await _userService.ChangePasswordAsync(request));
		}

		[HttpPut]
		[Authorize]
		public async Task<ActionResult<CurrentUserResponseModel>> Update([FromBody] UpdateUserRequestModel request)
		{
			return ResponseUtils.OkResult(await _userService.UpdateUserAsync(request));
		}

		[HttpPut("UpdateAvatar")]
		[Authorize]
		public async Task<ActionResult<string>> UpdateAvatar([FromBody] string image)
		{
			return ResponseUtils.OkResult(await _userService.UpdateUserAvatarAsync(image));
		}
	}
}