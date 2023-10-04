using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IUserService
  {
    public Task<UserContextModel> CreateUserStandardRegistrationAsync(RegisterRequestModel request);
    public Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo);
    public Task ResetPasswordAsync(ResetPasswordRequestModel request);
    public Task ChangePasswordAsync(ChangePasswordRequestModel request);
  }  
}