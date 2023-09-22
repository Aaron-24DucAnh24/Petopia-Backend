using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdoption.Business.Models;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.API.Controllers
{
  [ApiController]
  [Route("api/Authentication")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ICookieService _cookieService;
    private readonly IModelValidationService _modelValidationService;

    public AuthController(
      IAuthService authService,
      ICookieService cookieService,
      IUserService userService,
      IModelValidationService modelValidationService
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _userService = userService;
      _modelValidationService = modelValidationService;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync([FromBody] RegisterRequest request)
    {
      if(!await _modelValidationService.ValidateAsync(request, ModelState))
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
    public async Task<ActionResult<AuthenticationResponse>> LoginAsync([FromBody] LoginRequest request)
    {
      var result = await _authService.LoginAsync(request);
      _cookieService.SetAccessToken(result.AccessToken);
      return Ok(result);
    }

    [HttpPost("GoogleLogin")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> GoogleLoginAsync([FromBody] GoogleLoginRequest request)
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
    public async Task<ActionResult<string>> LogoutAsync()
    {
      await _authService.LogoutAsync();
      _cookieService.ClearAccessToken();
      return Ok("Success");
    }
  }
}