using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IUserService
  {
    public Task<CurrentUserResponseModel> GetCurrentUserAsync();
    public Task<CurrentUserResponseModel> GetOtherUserAsync(string userId);
		public Task<CurrentUserCoreResponseModel> GetCurrentUserCoreAsync();    
    public Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request);
    public Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo);
    public Task<bool> ResetPasswordAsync(ResetPasswordRequestModel request);
    public Task<bool> ChangePasswordAsync(ChangePasswordRequestModel request);
    public Task<CurrentUserResponseModel> UpdateUserAsync(UpdateUserRequestModel request);
    public Task<string> UpdateUserAvatarAsync(string image);
    public Task<string> GetUserNameAsync(Guid userId);
	}  
}