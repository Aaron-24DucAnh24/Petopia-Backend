using System.Data;
using Google.Apis.Auth;
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

    public async Task<User> CreateUserGoogleRegistrationAsync(GoogleLoginRequest request)
    {
      var payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId, new GoogleJsonWebSignature.ValidationSettings());
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);
      if (user != null)
      {
        return user;
      }
      user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = payload.Email,
        FirstName = payload.GivenName,
        LastName = payload.FamilyName,
        Password = string.Empty,
      });
      await CreateUserConnectionAsync(user);
      await UnitOfWork.SaveChangesAsync();
      return user;
    }

    public async Task<User> CreateUserStandardRegistrationAsync(RegisterRequest request)
    {
      if (await UnitOfWork.Users.FirstOrDefaultAsync(u => u.Email == request.Email) != null)
      {
        throw new DuplicateNameException();
      }
      var user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Password = request.Password,
      });
      await CreateUserConnectionAsync(user);
      await UnitOfWork.SaveChangesAsync();
      return user;
    }

    private async Task CreateUserConnectionAsync(User user)
    {
      await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
      {
        Id = user.Id,
        AccessToken = TokenUtil.GenerateAccessToken(user, Configuration),
        AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS),
      });
    }
  }
}