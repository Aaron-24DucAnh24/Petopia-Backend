using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.User;
using Petopia.Data.Entities;

namespace Petopia.Business.Interfaces
{
  public interface IUserService
  {
    public Task<User> CreateUserStandardRegistrationAsync(RegisterRequest request);
    public Task<User> CreateUserGoogleRegistrationAsync(GoogleUserInfo userInfo);
    public Task ResetPasswordAsync(ResetPasswordRequest request);
  }  
}