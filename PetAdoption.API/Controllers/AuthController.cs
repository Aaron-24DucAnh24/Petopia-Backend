using FluentValidation;
using FluentValidation.AspNetCore;
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
    private readonly IValidator<RegisterRequest> _registerRequestValidator;

    public AuthController(
      IAuthService authService,
      ICookieService cookieService,
      IUserService userService,
      IValidator<RegisterRequest> registerRequestValidator
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _userService = userService;
      _registerRequestValidator = registerRequestValidator;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync([FromBody] RegisterRequest request)
    {
      var validationResult = await _registerRequestValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
        return BadRequest(ModelState);
      } 
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
      var user = await _userService.CreateUserGoogleRegistrationAsync(request);
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

    [HttpGet("ValidateRecaptchaToken")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> ValidateRecaptchaTokenAsync([FromQuery] string token)
    {
      return Ok(await _authService.ValidateRecaptchaTokenAsync(token));
    }

    [HttpGet("TestFeature")]
    [Authorize]
    public ActionResult<string> TestAuthenticationAsync()
    {
      return Ok("Success");
    }
  }
}