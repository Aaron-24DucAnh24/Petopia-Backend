using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;

namespace Petopia.Business.Interfaces
{
  public interface IUserService
  {
    public Task<GetUserDetailsResponseModel> GetCurrentUserAsync();
    public Task<GetUserDetailsResponseModel> GetOtherUserAsync(string userId);
		public Task<CurrentUserCoreResponseModel> GetCurrentUserCoreAsync();    
    public Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request);
    public Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo);
    public Task<bool> ResetPasswordAsync(ResetPasswordRequestModel request);
    public Task<bool> ChangePasswordAsync(ChangePasswordRequestModel request);
    public Task<GetUserDetailsResponseModel> UpdateUserAsync(UpdateUserRequestModel request);
    public Task<string> UpdateUserAvatarAsync(string image);
    public Task<string> GetAddressAsync(string provinceCode, string districtCode, string wardCode, string street);
    public Task<string> GetUserNameAsync(Guid userId);
    public Task<bool> UpgradeAccountAsync(UpgradeAccountRequestModel request);
	}
}