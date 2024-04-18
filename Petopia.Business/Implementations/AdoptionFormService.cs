using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Adoption;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Notification;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
	public class AdoptionFormService : BaseService, IAdoptionFormService
	{
		private readonly IUserService _userService;
		private readonly INotificationService _notificationService;

		public AdoptionFormService(
			IServiceProvider provider,
			ILogger<AdoptionFormService> logger
		) : base(provider, logger)
		{
			_userService = provider.GetRequiredService<IUserService>();
			_notificationService = provider.GetRequiredService<INotificationService>();
		}

		public async Task<bool> ActOnAdoptionFormAsync(Guid formId, AdoptStatus status)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.Include(x => x.Pet)
				.AsTracking()
				.FirstOrDefaultAsync(x => x.Id == formId)
				?? throw new FormNotFoundException();

			string adopterName = await _userService.GetUserNameAsync(form.AdopterId);

			await _notificationService.CreateNoticationAsync(new CreateNotificationModel()
			{
				GoalId = form.Id,
				UserId = form.AdopterId,
				AdopterName = adopterName,
				PetName = form.Pet.Name,
				Type = NotificationType.Adoption,
				Status = status,
			});

			if(status == AdoptStatus.Adopted)
			{
				await _notificationService.CreateNoticationAsync(new CreateNotificationModel()
				{
					GoalId = form.Id,
					UserId = form.Pet.OwnerId,
					AdopterName = adopterName,
					PetName = form.Pet.Name,
					Type = NotificationType.Adoption,
					Status = status,
				});
			}

			form.Status = status;
			UnitOfWork.AdoptionForms.Update(form);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> CreateAdoptionFormAsync(CreateAdoptionRequestModel request)
		{
			Pet pet = await UnitOfWork.Pets.FirstAsync(x => x.Id == request.PetId);

			User user = await UnitOfWork.Users
				.AsTracking()
				.Include(x => x.UserIndividualAttributes)
				.Include(x => x.UserOrganizationAttributes)
				.FirstAsync(x => x.Id == UserContext.Id);

			string userName = user.Role == UserRole.Organization
				? user.UserOrganizationAttributes.OrganizationName
				: string.Join(" ", user.UserIndividualAttributes.FirstName, user.UserIndividualAttributes.LastName);

			AdoptionForm form = await UnitOfWork.AdoptionForms.CreateAsync(new AdoptionForm()
			{
				Id = Guid.NewGuid(),
				AdopterId = UserContext.Id,
				PetId = request.PetId,
				HouseType = request.HouseType,
				Message = request.Message,
				DelayDuration = request.AdoptTime,
				IsCreatedAt = DateTimeOffset.Now,
				IsUpdatedAt = DateTimeOffset.Now,
				IsOwnerBefore = request.IsOwnerBefore,
			});

			user.Phone = request.Phone;
			user.ProvinceCode = request.ProvinceCode;
			user.DistrictCode = request.DistrictCode;
			user.WardCode = request.WardCode;
			user.Street = request.Street;
			user.Address = await _userService.GetAddressAsync(
				user.ProvinceCode,
				user.DistrictCode,
				user.WardCode,
				user.Street
				);

			UnitOfWork.Users.Update(user);
			await UnitOfWork.SaveChangesAsync();

			await _notificationService.CreateNoticationAsync(new CreateNotificationModel()
			{
				GoalId = form.Id,
				UserId = pet.OwnerId,
				PetName = pet.Name,
				AdopterName = userName,
				Type = NotificationType.Adoption,
				Status = AdoptStatus.Pending,
			});

			return true;
		}

		public async Task<bool> DeleteAdoptionFormAsync(Guid formId)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.Include(x => x.Pet)
				.FirstAsync(x => x.Id == formId);

			string userName = await _userService.GetUserNameAsync(form.AdopterId);

			await _notificationService.CreateNoticationAsync(new CreateNotificationModel()
			{
				UserId = form.Pet.OwnerId,
				AdopterName = userName,
				PetName = form.Pet.Name,
				Type = NotificationType.Adoption,
				Status = AdoptStatus.Cancel,
			});

			UnitOfWork.AdoptionForms.Delete(form);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<DetailAdoptionFormResponseModel> GetAdoptionFormAsync(Guid formId)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.AsTracking()
				.Include(x => x.Pet)
				.FirstOrDefaultAsync(x => x.Id == formId)
				?? throw new FormNotFoundException();

			User adopter = await UnitOfWork.Users
				.Include(x => x.UserIndividualAttributes)
				.Include(x => x.UserOrganizationAttributes)
				.FirstAsync(x => x.Id == form.AdopterId);

			string adopterName = adopter.Role == UserRole.Organization
				? adopter.UserOrganizationAttributes.OrganizationName
				: string.Join(" ", adopter.UserIndividualAttributes.FirstName, adopter.UserIndividualAttributes.LastName);

			if (UserContext.Id == form.Pet.OwnerId)
			{
				form.IsSeen = true;
			}
			UnitOfWork.AdoptionForms.Update(form);
			await UnitOfWork.SaveChangesAsync();

			var result = Mapper.Map<DetailAdoptionFormResponseModel>(form);
			result.AdopterEmail = HashUtils.DecryptString(adopter.Email);
			result.AdopterPhone = adopter.Phone;
			result.AdopterRole = adopter.Role;
			result.AdopterName = adopterName;
			result.Address = adopter.Address;
			return result;
		}

		public async Task<List<AdoptionFormResponseModel>> GetAdoptionFormsIncomingAsync()
		{
			List<AdoptionForm> forms = await UnitOfWork.AdoptionForms
				.Include(x => x.Pet)
				.Where(x => x.Pet.OwnerId == UserContext.Id)
				.ToListAsync();
			List<AdoptionFormResponseModel> result = new();
			foreach (var form in forms)
			{
				string adopterName = await _userService.GetUserNameAsync(form.AdopterId);
				result.Add(new AdoptionFormResponseModel()
				{
					Id = form.Id,
					LastUpdatedAt = form.IsCreatedAt.CompareTo(form.IsUpdatedAt) > 0 ? form.IsCreatedAt : form.IsUpdatedAt,
					IsSeen = UserContext.Role == UserRole.SystemAdmin ? form.IsSeenByAdmin : form.IsSeen,
					Status = form.Status,
					PetName = form.Pet.Name,
					AdopterName = adopterName,
				});
			}
			result.OrderByDescending(x => x.LastUpdatedAt);
			return result;
		}

		public async Task<List<AdoptionFormResponseModel>> GetAdoptionFormsByUserIdAsync()
		{
			List<AdoptionForm> forms = await UnitOfWork.AdoptionForms
				.Include(x => x.Pet)
				.Where(x => x.AdopterId == UserContext.Id)
				.ToListAsync();
			List<AdoptionFormResponseModel> result = new();
			foreach (var form in forms)
			{
				string adopterName = await _userService.GetUserNameAsync(form.AdopterId);
				result.Add(new AdoptionFormResponseModel()
				{
					Id = form.Id,
					LastUpdatedAt = form.IsCreatedAt.CompareTo(form.IsUpdatedAt) > 0 ? form.IsCreatedAt : form.IsUpdatedAt,
					IsSeen = true,
					Status = form.Status,
					PetName = form.Pet.Name,
					AdopterName = adopterName,
				});
			}
			result.OrderByDescending(x => x.LastUpdatedAt);
			return result;
		}

		public async Task<bool> PreCheckAsync(Guid petId)
		{
			bool isOwned = await UnitOfWork.Pets.AnyAsync(x => x.Id == petId && x.OwnerId == UserContext.Id);
			if (isOwned)
			{
				throw new OwnedPetException();
			}

			bool isAdopted = await UnitOfWork.AdoptionForms.AnyAsync(
				x => x.AdopterId == UserContext.Id
				&& x.PetId == petId
				&& x.Status != AdoptStatus.Rejected
			);
			if (isAdopted)
			{
				throw new AdoptedPetException();
			}

			return true;
		}

		public async Task<int> CountUnreadRequestByUserAsync()
		{
			List<AdoptionForm> forms = await UnitOfWork.AdoptionForms
				.Include(x => x.Pet)
				.Where(x => x.Pet.OwnerId == UserContext.Id && !x.IsSeen)
				.ToListAsync();

			int result = forms.Where(x => !x.IsSeen).Count();

			return result;
		}
	}
}