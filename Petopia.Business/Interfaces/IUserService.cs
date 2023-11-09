using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IUserService
  {
    public Task<CurrentUserResponseModel> GetCurrentUserAsync();
    public Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request);
    public Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo);
    public Task<bool> ResetPasswordAsync(ResetPasswordRequestModel request);
    public Task<bool> ChangePasswordAsync(ChangePasswordRequestModel request);
  }  
}