using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IAuthService
  {
    public Task<JwtTokensModel> LoginAsync(LoginRequestModel request);
    public Task<JwtTokensModel> LoginAsync(UserContextModel model);
    public Task<bool> LogoutAsync();
    public Task<string> CacheRegisterRequestAsync(RegisterRequestModel request);
    public Task<GoogleUserModel> ValidateGoogleLoginTokenAsync(string token);
    public Task ValidateGoogleRecaptchaTokenAsync(string token);
    public UserContextModel ValidateAccessToken(string token);
    public UserContextModel ValidateRefreshToken(string? token);
    public string GetGoogleRecaptchaSiteKey();
    public string GetGoogleAuthClientId();
    public Task<JwtTokensModel> AdminLoginAsync(LoginRequestModel request);
  }
}