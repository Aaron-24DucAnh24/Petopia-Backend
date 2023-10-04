using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Exceptions;
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

    public async Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(userInfo.Email));
      if (user == null)
      {
        user = await UnitOfWork.Users.CreateAsync(new User()
        {
          Email = HashUtils.HashString(userInfo.Email),
          FirstName = userInfo.GivenName,
          LastName = userInfo.FamilyName,
          Password = string.Empty,
          Image = userInfo.Picture
        });
        await UnitOfWork.SaveChangesAsync();
      }
      return new UserContextModel()
      {
        Id = user.Id,
        Role = user.Role,
        Email = userInfo.Email
      };
    }

    public async Task<UserContextModel> CreateUserStandardRegistrationAsync(RegisterRequestModel request)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(request.Email))
        ?? throw new UsedEmailException();
      user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Email = HashUtils.HashString(request.Email),
        FirstName = request.FirstName,
        LastName = request.LastName,
        Password = HashUtils.HashPassword(request.Password),
      });
      await UnitOfWork.SaveChangesAsync();
      return new UserContextModel()
      {
        Id = user.Id,
        Role = user.Role,
        Email = request.Email
      };
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestModel request)
    {
      var user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == request.UserId);
      if (user == null
      || user.ResetPasswordTokenExpirationDate < DateTimeOffset.Now
      || user.ResetPasswordToken != request.ResetPasswordToken)
      {
        throw new InvalidPasswordTokenException();
      }
      user.Password = HashUtils.HashPassword(request.Password);
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(ChangePasswordRequestModel request)
    {
      var user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == UserContext.Id);
      if (user == null || !HashUtils.VerifyHashedPassword(user.Password, request.OldPassword))
      {
        throw new InvalidPasswordTokenException();
      }
      user.Password = HashUtils.HashPassword(request.NewPassword);
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
    }
  }
}