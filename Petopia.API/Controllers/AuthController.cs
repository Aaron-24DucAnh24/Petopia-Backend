using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Authentication")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ICookieService _cookieService;

    public AuthController(
      IAuthService authService,
      ICookieService cookieService,
      IUserService userService
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _userService = userService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponseModel>> Register([FromBody] RegisterRequestModel request)
    {
      await _authService.ValidateGoogleRecaptchaTokenAsync(request.GoogleRecaptchaToken);
      var user = await _userService.CreateUserStandardRegistrationAsync(request);
      var result = await _authService.LoginAsync(user);
      _cookieService.SetAccessToken(result.AccessToken);
      return Ok(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponseModel>> Login([FromBody] LoginRequestModel request)
    {
      var result = await _authService.LoginAsync(request);
      _cookieService.SetAccessToken(result.AccessToken);
      return Ok(result);
    }

    [HttpPost("GoogleLogin")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponseModel>> GoogleLogin([FromBody] GoogleLoginRequestModel request)
    {
      var googleUserInfo = await _authService.ValidateGoogleLoginTokenAsync(request.TokenId);
      var user = await _userService.CreateUserGoogleRegistrationAsync(googleUserInfo);
      var result = await _authService.LoginAsync(user);
      _cookieService.SetAccessToken(result.AccessToken);
      return result;
    }

    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponseModel>> RefreshToken([FromBody] string token)
    {
      var user = _authService.ValidateRefreshToken(token);
      var result = await _authService.LoginAsync(user);
      _cookieService.SetAccessToken(result.AccessToken);
      return result;
    }

    [HttpGet("Logout")]
    [Authorize]
    public async Task<ActionResult<bool>> Logout()
    {
      var result = await _authService.LogoutAsync();
      _cookieService.ClearAccessToken();
      return Ok(result);
    }
  }
}