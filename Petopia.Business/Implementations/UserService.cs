using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Authentication;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Pet;
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
			CurrentUserResponseModel result = user.Role == UserRole.Organization
				? await GetCurrentOrganizationAsync(user)
				: await GetCurrentIndividualAsync(user);
			List<Pet> pets = await UnitOfWork.Pets.Where(x => x.OwnerId == UserContext.Id).ToListAsync();
			result.Pets = Mapper.Map<List<PetResponseModel>>(pets);
			return result;
		}

		public async Task<CurrentUserCoreResponseModel> GetCurrentUserCoreAsync()
		{
			User user = await UnitOfWork.Users
				.Include(x => x.UserIndividualAttributes)
				.Include(x => x.UserOrganizationAttributes)
				.FirstAsync(x => x.Id == UserContext.Id);
			var result = Mapper.Map<CurrentUserCoreResponseModel>(user);
			result.Email = HashUtils.DecryptString(result.Email);
			result.Name = user.Role == UserRole.Organization
				? user.UserOrganizationAttributes.OrganizationName
				: string.Join(" ", user.UserIndividualAttributes.FirstName, user.UserIndividualAttributes.LastName);
			return result;
		}

		public async Task<UserContextModel> CreateUserSelfRegistrationAsync(ValidateRegisterRequestModel request)
		{
			RegisterRequestModel cacheData = CacheManager.Instance.Get<RegisterRequestModel>(request.ValidateRegisterToken)
				?? throw new InvalidRegisterTokenException();
			User user = await UnitOfWork.Users.CreateAsync(new User()
			{
				Id = Guid.NewGuid(),
				Email = HashUtils.EnryptString(cacheData.Email),
				Password = HashUtils.HashPassword(cacheData.Password),
				IsCreatedAt = DateTimeOffset.Now,
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
			User? user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == HashUtils.EnryptString(userInfo.Email));
			if (user == null)
			{
				user = await UnitOfWork.Users.CreateAsync(new User()
				{
					Id = Guid.NewGuid(),
					Email = HashUtils.EnryptString(userInfo.Email),
					Password = string.Empty,
					Image = userInfo.Picture,
					IsCreatedAt = DateTimeOffset.Now,
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
				throw new InvalidPasswordException();
			}
			user.Password = HashUtils.HashPassword(request.NewPassword);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		#region private

		private async Task<CurrentOrganizationResponseModel> GetCurrentOrganizationAsync(User user)
		{
			user.UserOrganizationAttributes = await UnitOfWork.UserOrganizationAttributes.FirstAsync(x => x.Id == user.Id);
			var result = Mapper.Map<CurrentOrganizationResponseModel>(user);
			result.Email = HashUtils.DecryptString(result.Email);
			return result;
		}

		private async Task<CurrentIndividualResponseModel> GetCurrentIndividualAsync(User user)
		{
			user.UserIndividualAttributes = await UnitOfWork.UserIndividualAttributes.FirstAsync(x => x.Id == user.Id);
			var result = Mapper.Map<CurrentIndividualResponseModel>(user);
			result.Email = HashUtils.DecryptString(result.Email);
			return result;
		}

		#endregion
	}
}