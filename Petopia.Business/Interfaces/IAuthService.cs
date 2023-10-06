using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IAuthService
  {
    public Task<AuthenticationResponseModel> LoginAsync(LoginRequestModel request);
    public Task<AuthenticationResponseModel> LoginAsync(UserContextModel model);
    public Task<GoogleUserModel> ValidateGoogleLoginTokenAsync(string token);
    public Task ValidateGoogleRecaptchaTokenAsync(string token);
    public Task<bool> LogoutAsync();
    public UserContextModel ValidateAccessToken(string token);
    public UserContextModel ValidateRefreshToken(string token);
  }
}