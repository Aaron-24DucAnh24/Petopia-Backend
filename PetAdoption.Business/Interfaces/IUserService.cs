using PetAdoption.Business.Models.Authentication;
using PetAdoption.Business.Models.User;
using PetAdoption.Data.Entities;

namespace PetAdoption.Business.Interfaces
{
  public interface IUserService
  {
    public Task<User> CreateUserStandardRegistrationAsync(RegisterRequest request);
    public Task<User> CreateUserGoogleRegistrationAsync(GoogleUserInfo request);
  }  
}