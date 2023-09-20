using PetAdoption.Business.Models;
using PetAdoption.Data.Entities;

namespace PetAdoption.Business.Interfaces
{
  public interface IAuthService
  {
    public Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    public Task<string> ValidateRecaptchaTokenAsync(string token);
    public Task<bool> LogoutAsync();
    public bool ValidateAccessToken(string token);
  }
}