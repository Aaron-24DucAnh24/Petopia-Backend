using PetAdoption.Business.Models;
using PetAdoption.Data.Entities;

namespace PetAdoption.Business.Interfaces
{
  public interface IUserService
  {
    public Task<User> CreateUserStandardRegistrationAsync(RegisterRequest request);
    public Task<User> CreateUserGoogleRegistrationAsync(GoogleUserInfo request);
  }  
}