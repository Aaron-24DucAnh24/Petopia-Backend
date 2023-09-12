using PetAdoption.Business.Models;

namespace PetAdoption.Business.Interfaces
{
  public interface IAuthService
  {
    public Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    public Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
    public Task<string> ValidateRecaptchaTokenAsync(string token);
    public Task<bool> LogoutAsync();
    public bool ValidateAccessToken(string token);
  }
}