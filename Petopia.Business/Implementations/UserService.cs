using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Setting;
using Petopia.Business.Models.User;
using Petopia.Business.Utils;
using Petopia.Data.Entities;

namespace Petopia.Business.Implementations
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
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == request.Email)
        ?? throw new UsedEmailException();
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

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
      var user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == request.UserId && x.ResetPasswordToken == request.ResetPasswordToken);
      if (user == null || user.ResetPasswordTokenExpirationDate < DateTimeOffset.Now)
      {
        throw new InvalidPasswordToken();
      }
      user.Password = request.Password;
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
      var user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == UserContext.Id && x.Password == request.OldPassword)
        ?? throw new InvalidCredentialException();
      user.Password = request.NewPassword;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
    }

    private async Task<UserConnection> CreateUserConnectionAsync(User user)
    {
      var tokenSetting = Configuration.GetSection(AppSettingKey.TOKEN).Get<TokenSettingModel>()
        ?? throw new ConfigurationErrorsException();
      return await UnitOfWork.UserConnections.CreateAsync(new UserConnection()
      {
        Id = user.Id,
        AccessToken = TokenUtil.CreateAccessToken(user, Configuration),
        AccessTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenConfig.ACCESS_TOKEN_EXPIRATION_DAYS),
      });
    }
  }
}