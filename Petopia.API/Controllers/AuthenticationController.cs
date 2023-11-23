using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Email;
using Petopia.Business.Models.User;
using Petopia.Business.Utils;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Authentication")]
  public class AuthenticationController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ICookieService _cookieService;
    private readonly IEmailService _emailService;
    private readonly IEmailJobService _emailJobService;

    public AuthenticationController(
      IAuthService authService,
      ICookieService cookieService,
      IUserService userService,
      IEmailService emailService,
      IEmailJobService emailJobService
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _userService = userService;
      _emailService = emailService;
      _emailJobService = emailJobService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> Register([FromBody] RegisterRequestModel request)
    {
      await _authService.ValidateGoogleRecaptchaTokenAsync(request.GoogleRecaptchaToken);
      CacheRegisterRequestModel cacheData =  await _authService.CacheRegisterRequestAsync(request);
      MailDataModel mailMessage = await _emailService.CreateValidateRegisterMailDataAsync(request.Email, cacheData.RegisterToken);
      _emailJobService.SendMail(mailMessage);
      return ResponseUtils.OkResult(true);
    }

    [HttpGet("ValidateRegister")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> ValidateRegisterEmail([FromQuery] ValidateRegisterRequestModel request)
    {
      await _userService.CreateUserSelfRegistrationAsync(request);
      return ResponseUtils.OkResult(true);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> Login([FromBody] LoginRequestModel request)
    {
      JwtTokensModel result = await _authService.LoginAsync(request);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return ResponseUtils.OkResult(true);
    }

    [HttpPost("GoogleLogin")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> GoogleLogin([FromBody] GoogleLoginRequestModel request)
    {
      GoogleUserModel googleUserInfo = await _authService.ValidateGoogleLoginTokenAsync(request.TokenId);
      UserContextModel user = await _userService.CreateUserGoogleRegistrationAsync(googleUserInfo);
      JwtTokensModel result = await _authService.LoginAsync(user);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return ResponseUtils.OkResult(true);
    }

    [HttpGet("RefreshToken")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> RefreshToken()
    {
      UserContextModel user = _authService.ValidateRefreshToken();
      JwtTokensModel result = await _authService.LoginAsync(user);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return ResponseUtils.OkResult(true);
    }

    [HttpGet("Logout")]
    [Authorize]
    public async Task<ActionResult<bool>> Logout()
    {
      bool result = await _authService.LogoutAsync();
      _cookieService.ClearJwtTokens();
      return ResponseUtils.OkResult(result);
    }
  }
}