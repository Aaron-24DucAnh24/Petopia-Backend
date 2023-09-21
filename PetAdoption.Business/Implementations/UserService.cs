using Microsoft.Extensions.Logging;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Models;
using PetAdoption.Business.Utils;
using PetAdoption.Data.Entities;

namespace PetAdoption.Business.Implementations
{
  public class UserService : BaseService, IUserService
  {
    public UserService(
      IServiceProvider provider,
      ILogger<UserService> logger
    ) : base(provider, logger)
    {
    }

    public async Task<User> CreateUserGoogleRegistrationAsync(GoogleUserInfo userInfo)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == userInfo.Email);
      if (user != null)
      {
        return user;
      }
      user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = userInfo.Email,
        FirstName = userInfo.GivenName,
        LastName = userInfo.FamilyName,
        Password = string.Empty,
        Image = userInfo.Picture
      });
      await CreateUserConnectionAsync(user);
      await UnitOfWork.SaveChangesAsync();
      return user;
    }

    public async Task<User> CreateUserStandardRegistrationAsync(RegisterRequest request)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
      if (user != null)
      {
        return user;
      }
      user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Password = string.Empty,
      });
      await CreateUserConnectionAsync(user);
      await UnitOfWork.SaveChangesAsync();
      return user;
    }

    private async Task<UserConnection> CreateUserConnectionAsync(User user)
    {
      return await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
      {
        Id = user.Id,
        AccessToken = TokenUtil.GenerateAccessToken(user, Configuration),
        AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS),
      });
    }
  }
}