using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.User;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

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

    public async Task<CurrentUserResponseModel> GetCurrentUserAsync()
    {
      User user = await UnitOfWork.Users.FirstAsync(x => x.Id == UserContext.Id);
      if (user.Role == UserRole.Organization)
      {
        user.UserOrganizationAttributes = await UnitOfWork.UserOrganizationAttributes.FirstAsync(x => x.Id == user.Id);
        return Mapper.Map<CurrentOrganizationResponseModel>(user);
      }
      user.UserIndividualAttributes = await UnitOfWork.UserIndividualAttributes.FirstAsync(x => x.Id == user.Id);
      return Mapper.Map<CurrentIndividualResponseModel>(user);
    }

    public async Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request)
    {
      string cacheKey = HashUtils.HashString(request.Email);
      CacheRegisterRequestModel? cacheData = CacheManager.Instance.Get<CacheRegisterRequestModel>(cacheKey);
      if (cacheData == null || cacheData.RegisterToken != request.ValidateRegisterToken)
      {
        throw new InvalidRegisterTokenException();
      }
      User user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Id = Guid.NewGuid(),
        Email = HashUtils.HashString(cacheData.Request.Email),
        Password = HashUtils.HashPassword(cacheData.Request.Password),
      });
      UserIndividualAttributes attributes = await UnitOfWork.UserIndividualAttributes.CreateAsync(new UserIndividualAttributes()
      {
        Id = user.Id,
        FirstName = cacheData.Request.FirstName,
        LastName = cacheData.Request.LastName
      });
      await UnitOfWork.SaveChangesAsync();
      CacheManager.Instance.Remove(cacheKey);
      return new UserContextModel()
      {
        Id = user.Id,
        Role = user.Role,
        Email = user.Email
      };
    }

    public async Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo)
    {
      User? user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(userInfo.Email));
      if (user == null)
      {
        user = await UnitOfWork.Users.CreateAsync(new User()
        {
          Id = Guid.NewGuid(),
          Email = HashUtils.HashString(userInfo.Email),
          Password = string.Empty,
          Image = userInfo.Picture
        });
        UserIndividualAttributes attributes = await UnitOfWork.UserIndividualAttributes.CreateAsync(new UserIndividualAttributes()
        {
          Id = user.Id,
          FirstName = userInfo.GivenName,
          LastName = userInfo.FamilyName
        });
        await UnitOfWork.SaveChangesAsync();
      }
      if(!string.IsNullOrEmpty(user.Password))
      {
        throw new WrongLoginMethodException();
      }
      return new UserContextModel()
      {
        Id = user.Id,
        Role = user.Role,
        Email = userInfo.Email
      };
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequestModel request)
    {
      User? user = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(request.Email));
      if (user == null
      || user.ResetPasswordTokenExpirationDate < DateTimeOffset.Now
      || user.ResetPasswordToken != request.ResetPasswordToken)
      {
        throw new InvalidPasswordTokenException();
      }
      user.Password = HashUtils.HashPassword(request.Password);
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now;
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequestModel request)
    {
      User user = await UnitOfWork.Users
        .AsTracking()
        .FirstAsync(x => x.Id == UserContext.Id);
      if (!HashUtils.VerifyHashedPassword(user.Password, request.OldPassword))
      {
        throw new InvalidPasswordTokenException();
      }
      user.Password = HashUtils.HashPassword(request.NewPassword);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }
  }
}