using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
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
    private readonly ICookieService _cookieService;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly IValidator<RegisterRequest> _registerRequestValidator;

    public AuthController(
      IAuthService authService,
      ICookieService cookieService,
      IValidator<LoginRequest> loginRequestValidator,
      IValidator<RegisterRequest> registerRequestValidator
    )
    {
      _authService = authService;
      _cookieService = cookieService;
      _loginRequestValidator = loginRequestValidator;
      _registerRequestValidator = registerRequestValidator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync([FromBody] RegisterRequest request)
    {
      ValidationResult validationResult = await _registerRequestValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
        return BadRequest(ModelState);
      }

      AuthenticationResponse result = await _authService.RegisterAsync(request);
      _cookieService.SetAccessToken(result.AccessToken);

      return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResponse>> LoginAsync([FromBody] LoginRequest request)
    {
      ValidationResult validationResult = await _loginRequestValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        validationResult.AddToModelState(ModelState);
        return BadRequest(ModelState);
      }

      AuthenticationResponse result = await _authService.LoginAsync(request);
      _cookieService.SetAccessToken(result.AccessToken);

      return Ok(result);
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<ActionResult<string>> LogoutAsync()
    {
      await _authService.LogoutAsync();
      _cookieService.ClearAccessToken();
      return Ok("Success");
    }

    [HttpGet("validate-recaptcha-token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> ValidateRecaptchaTokenAsync([FromQuery] string token)
    {
      return Ok(await _authService.ValidateRecaptchaTokenAsync(token));
    }

    [HttpGet("test-feature")]
    [Authorize]
    public ActionResult<string> TestAuthenticationAsync()
    {
      return Ok("Success");
    }
  }
}