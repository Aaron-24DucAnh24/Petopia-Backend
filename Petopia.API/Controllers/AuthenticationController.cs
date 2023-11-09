using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;

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
      // await _authService.ValidateGoogleRecaptchaTokenAsync(request.GoogleRecaptchaToken);
      var cacheData =  _authService.CacheRegisterRequest(request);
      var mailMessage = _emailService.CreateValidateRegisterMailDataAsync(request.Email, cacheData.RegisterToken);
      _emailJobService.SendMail(mailMessage);
      return Ok(true);
    }

    [HttpGet("ValidateRegister")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> ValidateRegisterEmail([FromQuery] ValidateRegisterRequestModel request)
    {
      await _userService.CreateUserSelfRegistrationAsync(request);
      return Ok("Đăng ký thành công");
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> Login([FromBody] LoginRequestModel request)
    {
      var result = await _authService.LoginAsync(request);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return Ok(true);
    }

    [HttpPost("GoogleLogin")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> GoogleLogin([FromBody] GoogleLoginRequestModel request)
    {
      var googleUserInfo = await _authService.ValidateGoogleLoginTokenAsync(request.TokenId);
      var user = await _userService.CreateUserGoogleRegistrationAsync(googleUserInfo);
      var result = await _authService.LoginAsync(user);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return Ok(true);
    }

    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> RefreshToken([FromBody] string token)
    {
      var user = _authService.ValidateRefreshToken(token);
      var result = await _authService.LoginAsync(user);
      _cookieService.SetJwtTokens(result.AccessToken, result.RefreshToken);
      return Ok(true);
    }

    [HttpGet("Logout")]
    [Authorize]
    public async Task<ActionResult<bool>> Logout()
    {
      var result = await _authService.LogoutAsync();
      _cookieService.ClearJwtTokens();
      return Ok(result);
    }
  }
}