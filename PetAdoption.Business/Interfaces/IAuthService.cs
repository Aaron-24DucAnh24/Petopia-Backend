using PetAdoption.Business.Models;

namespace PetAdoption.Business.Interfaces
{
  public interface IAuthService
  {
    public Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    public Task<GoogleUserInfo> ValidateGoogleLoginTokenAsync(string token);
    public Task ValidateGoogleRecaptchaTokenAsync(string token);
    public Task<bool> LogoutAsync();
    public bool ValidateAccessToken(string token);
  }
}