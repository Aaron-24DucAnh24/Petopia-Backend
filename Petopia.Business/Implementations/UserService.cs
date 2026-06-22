using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Extensions;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Notification;
using Petopia.Business.Models.User;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class UserService : BaseService, IUserService
  {
    private readonly INotificationService _notificationService;

    public UserService(
      IServiceProvider provider,
      ILogger<UserService> logger
    ) : base(provider, logger)
    {
      _notificationService = provider.GetRequiredService<INotificationService>();
    }

    public async Task<GetUserDetailsResponseModel> GetCurrentUserAsync()
    {
      return await GetUserInfoAsync(UserContext.Id);
    }

    public async Task<GetUserDetailsResponseModel> GetOtherUserAsync(string userId)
    {
      if (Guid.TryParse(userId, out Guid id))
      {
        return await GetUserInfoAsync(id);
      }
      throw new UserNotFoundException();
    }

    public async Task<CurrentUserCoreResponseModel> GetCurrentUserCoreAsync()
    {
      User user = await UnitOfWork.Users
        .WithAttributes()
        .FirstAsync(x => x.Id == UserContext.Id);
      var result = Mapper.Map<CurrentUserCoreResponseModel>(user);
      result.Email = HashUtils.DecryptString(
        user.Role == UserRole.Organization
        ? user.UserOrganizationAttributes.Email
        : user.Email
      );
      result.Name = user.Role == UserRole.Organization
        ? user.UserOrganizationAttributes.OrganizationName
        : string.Join(" ", user.UserIndividualAttributes.FirstName, user.UserIndividualAttributes.LastName);
      return result;
    }

    public async Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request)
    {
      var cacheData = CacheManager.Instance
        .Get<RegisterRequestModel>(request.ValidateRegisterToken)
        ?? throw new InvalidRegisterTokenException();
      var user = await UnitOfWork.Users.CreateAsync(new User()
      {
        Id = Guid.NewGuid(),
        Email = HashUtils.EnryptString(cacheData.Email),
        Password = HashUtils.HashPassword(cacheData.Password),
        IsCreatedAt = DateTimeOffset.UtcNow,
        BirthDate = DateTimeOffset.Parse(cacheData.BirthDate),
      });
      await UnitOfWork.UserIndividualAttributes.CreateAsync(new UserIndividualAttributes()
      {
        Id = user.Id,
        FirstName = cacheData.FirstName,
        LastName = cacheData.LastName

      });
      await UnitOfWork.SaveChangesAsync();
      CacheManager.Instance.Remove(request.ValidateRegisterToken);
      return new UserContextModel()
      {
        Id = user.Id,
        Role = user.Role,
        Email = user.Email
      };
    }

    public async Task<UserContextModel> CreateUserGoogleRegistrationAsync(GoogleUserModel userInfo)
    {
      User? user = await FindUserByEmailAsync(userInfo.Email);
      if (user == null)
      {
        user = await UnitOfWork.Users.CreateAsync(new User()
        {
          Id = Guid.NewGuid(),
          Email = HashUtils.EnryptString(userInfo.Email),
          Password = string.Empty,
          Image = userInfo.Picture,
          IsCreatedAt = DateTimeOffset.UtcNow,
        });
        UserIndividualAttributes attributes = await UnitOfWork.UserIndividualAttributes.CreateAsync(new UserIndividualAttributes()
        {
          Id = user.Id,
          FirstName = userInfo.GivenName,
          LastName = userInfo.FamilyName
        });
        await UnitOfWork.SaveChangesAsync();
      }
      if (!string.IsNullOrEmpty(user.Password))
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
        .FirstOrDefaultAsync(x => x.Email == HashUtils.EnryptString(request.Email));
      if (user == null
      || user.ResetPasswordTokenExpirationDate < DateTimeOffset.UtcNow
      || user.ResetPasswordToken != request.ResetPasswordToken)
      {
        throw new InvalidPasswordTokenException();
      }
      user.Password = HashUtils.HashPassword(request.Password);
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.UtcNow;
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequestModel request)
    {
      User user = await UnitOfWork.Users
        .AsTracking()
        .FirstAsync(x => x.Id == UserContext.Id);
      if (string.IsNullOrEmpty(user.Password))
      {
        throw new WrongLoginMethodException();
      }
      if (!HashUtils.VerifyHashedPassword(user.Password, request.OldPassword))
      {
        throw new IncorrectPasswordException();
      }
      if (HashUtils.VerifyHashedPassword(user.Password, request.NewPassword))
      {
        throw new InvalidPasswordException();
      }
      user.Password = HashUtils.HashPassword(request.NewPassword);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    public async Task<GetUserDetailsResponseModel> UpdateUserAsync(UpdateUserRequestModel request)
    {
      User user = await UnitOfWork.Users
        .AsTracking()
        .WithAttributes()
        .Where(x => x.Id == UserContext.Id)
        .FirstAsync();

      user.Phone = request.Phone;
      user.DistrictCode = request.DistrictCode;
      user.ProvinceCode = request.ProvinceCode;
      user.WardCode = request.WardCode;
      user.Street = request.Street;
      user.BirthDate = request.BirthDate;

      user.Address = await GetAddressAsync(
        request.ProvinceCode,
        request.DistrictCode,
        request.WardCode,
        request.Street
      );

      if (UserContext.Role != UserRole.Organization)
      {
        user.UserIndividualAttributes.FirstName = request.FirstName;
        user.UserIndividualAttributes.LastName = request.LastName;
      }
      else
      {
        user.UserOrganizationAttributes.Description = request.Description;
        user.UserOrganizationAttributes.Website = request.Website;
        user.UserOrganizationAttributes.OrganizationName = request.OrganizationName;
        user.UserOrganizationAttributes.Type = request.Type;
      }

      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();
      return await GetCurrentUserAsync();
    }

    public async Task<string> UpdateUserAvatarAsync(IFormFile image)
    {
      var storageService = ServiceProvider.GetRequiredService<IStorageService>();
      var imageUrl = await storageService.UploadFileAsync(Constants.STORAGE_CONTAINER_IMAGE, image);

      // Upload new image
      if (string.IsNullOrEmpty(imageUrl)) throw new DomainException("");

      var user = await UnitOfWork.Users
        .AsTracking()
        .Where(x => x.Id == UserContext.Id)
        .FirstAsync();

      // Remove old image
      if (!string.IsNullOrEmpty(user.Image)
        && !await storageService.RemoveFileAsync(user.Image)) throw new DomainException("");

      // Update user image
      user.Image = imageUrl;
      UnitOfWork.Users.Update(user);
      await UnitOfWork.SaveChangesAsync();

      return imageUrl;
    }

    public async Task<string> GetAddressAsync(string provinceCode, string districtCode, string wardCode, string street)
    {
      string province = (await UnitOfWork.Provinces
        .Where(x => x.Code == provinceCode)
        .FirstOrDefaultAsync()
        ?? throw new WrongLocationCodeException())
        .Name;
      string district = (await UnitOfWork.Districts
        .Where(x => x.Code == districtCode)
        .FirstOrDefaultAsync()
        ?? throw new WrongLocationCodeException())
        .Name;
      string ward = (await UnitOfWork.Wards
        .Where(x => x.Code == wardCode)
        .FirstOrDefaultAsync()
        ?? throw new WrongLocationCodeException())
        .Name;
      return string.Join(", ", street, ward, district, province);
    }

    public async Task<bool> UpgradeAccountAsync(UpgradeAccountRequestModel request)
    {
      string address = await GetAddressAsync(
        request.ProvinceCode,
        request.DistrictCode,
        request.WardCode,
        request.Street
      );

      var upgradeForm = new UpgradeForm()
      {
        Id = Guid.NewGuid(),
        EntityName = request.EntityName,
        OrganizationName = request.OrganizationName,
        Email = request.Email,
        Phone = request.Phone,
        PrivinceCode = request.ProvinceCode,
        DistrictCode = request.DistrictCode,
        WardCode = request.WardCode,
        Street = request.Street,
        Address = address,
        Website = request.Website,
        TaxCode = request.TaxCode,
        Type = request.Type,
        Description = request.Description,
        IsCreatedAt = DateTimeOffset.UtcNow,
        UserId = UserContext.Id,
      };

      await UnitOfWork.UpgradeForms.CreateAsync(upgradeForm);
      await UnitOfWork.SaveChangesAsync();

      await _notificationService.CreateForAdminsAsync(new CreateNotificationModel
      {
        ReferenceId = upgradeForm.Id,
        ReferenceType = ReferenceType.UpgradeForm,
        Title = "Yêu cầu nâng cấp mới",
        Content = $"Có yêu cầu nâng cấp tài khoản mới từ \"{request.OrganizationName}\".",
        Type = NotificationType.UpgradeRequestCreated,
      });

      return true;
    }

    public async Task<UserUpgradeResponseModel[]> GetUpgradeRequestsAsync()
    {
      var requests = await UnitOfWork.UpgradeForms
        .Where(x => x.UserId == UserContext.Id)
        .OrderByDescending(x => x.IsCreatedAt)
        .ToArrayAsync();
      return requests.Select(x => Mapper.Map<UserUpgradeResponseModel>(x)).ToArray();
    }

    public async Task<UserUpgradeResponseModel> GetUpgradeRequestAsync(Guid id)
    {
      UpgradeForm form = await UnitOfWork.UpgradeForms
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserContext.Id)
        ?? throw new UpgradeRequestNotFoundException();
      return Mapper.Map<UserUpgradeResponseModel>(form);
    }

    public async Task<bool> CancelUpgradeRequestAsync(Guid id)
    {
      UpgradeForm form = await UnitOfWork.UpgradeForms
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserContext.Id)
        ?? throw new UpgradeRequestNotFoundException();
      if (form.Status != UpgradeStatus.Pending)
      {
        throw new UpgradeRequestNotPendingException();
      }
      form.Status = UpgradeStatus.Cancelled;
      UnitOfWork.UpgradeForms.Update(form);
      await UnitOfWork.SaveChangesAsync();
      return true;
    }

    #region private

    private async Task<GetUserDetailsResponseModel> GetUserInfoAsync(Guid userId)
    {
      var user = await UnitOfWork.Users
        .FirstOrDefaultAsync(x => x.Id == userId)
        ?? throw new UserNotFoundException();
      GetUserDetailsResponseModel result = user.Role == UserRole.Organization
        ? await GetCurrentOrganizationAsync(user)
        : await GetCurrentIndividualAsync(user);
      return result;
    }

    private async Task<CurrentOrganizationResponseModel> GetCurrentOrganizationAsync(User user)
    {
      user.UserOrganizationAttributes = await UnitOfWork.UserOrganizationAttributes.FirstAsync(x => x.Id == user.Id);
      var result = Mapper.Map<CurrentOrganizationResponseModel>(user);
      result.Email = HashUtils.DecryptString(user.UserOrganizationAttributes.Email);
      return result;
    }

    private async Task<CurrentIndividualResponseModel> GetCurrentIndividualAsync(User user)
    {
      user.UserIndividualAttributes = await UnitOfWork.UserIndividualAttributes.FirstOrDefaultAsync(x => x.Id == user.Id);
      var result = Mapper.Map<CurrentIndividualResponseModel>(user);
      result.Email = HashUtils.DecryptString(result.Email);
      return result;
    }

    #endregion
  }
}