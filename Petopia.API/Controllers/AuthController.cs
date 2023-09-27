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
    private readonly IValidationService _validationService;

    public AuthController(
      IAuthService authService,
      ICookieService cookieService,
      IUserService userService,
      IValidationService validationService
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _userService = userService;
      _validationService = validationService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Register([FromBody] RegisterRequest request)
    {
      if(!await _validationService.ValidateAsync(request, ModelState))
      {
        return BadRequest(ModelState);
      }
      await _authService.ValidateGoogleRecaptchaTokenAsync(request.GoogleRecaptchaToken);
      var user = await _userService.CreateUserStandardRegistrationAsync(request);
      var result = await _authService.LoginAsync(new LoginRequest
      {
        Email = user.Email,
        Password = user.Password
      });
      _cookieService.SetAccessToken(result.AccessToken);
      return Ok(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
    {
      var result = await _authService.LoginAsync(request);
      _cookieService.SetAccessToken(result.AccessToken);
      return Ok(result);
    }

    [HttpPost("GoogleLogin")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
      var googleUserInfo = await _authService.ValidateGoogleLoginTokenAsync(request.TokenId);
      var user = await _userService.CreateUserGoogleRegistrationAsync(googleUserInfo);
      var result = await _authService.LoginAsync(new LoginRequest
      {
        Email = user.Email,
        Password = user.Password
      });
      _cookieService.SetAccessToken(result.AccessToken);
      return result;
    }

    [HttpGet("Logout")]
    [Authorize]
    public async Task<ActionResult<string>> Logout()
    {
      await _authService.LogoutAsync();
      _cookieService.ClearAccessToken();
      return Ok("Success");
    }
  }
}