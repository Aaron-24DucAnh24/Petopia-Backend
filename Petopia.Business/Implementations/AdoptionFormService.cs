using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Adoption;
using Petopia.Business.Models.Exceptions;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
	public class AdoptionFormService : BaseService, IAdoptionFormService
	{
		private readonly IUserService _userService;

		public AdoptionFormService(
			IServiceProvider provider,
			ILogger<AdoptionFormService> logger
		) : base(provider, logger)
		{
			_userService = provider.GetRequiredService<IUserService>();
		}

		public async Task<bool> ActOnAdoptionFormAsync(Guid formId, AdoptStatus status)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.AsTracking()
				.FirstOrDefaultAsync(x => x.Id == formId)
				?? throw new FormNotFoundException();
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

			await UnitOfWork.AdoptionForms.CreateAsync(new AdoptionForm()
			{
				Id = Guid.NewGuid(),
				AdopterId = UserContext.Id,
				PetId = request.PetId,
				HouseType = request.HouseType,
				Message = request.Message,
				DelayDuration = request.AdoptTime,
				IsCreatedAt = DateTimeOffset.Now,
				IsUpdatedAt = DateTimeOffset.Now,
				Name = string.Join("Đơn nhận nuôi ", pet.Name, " từ ", userName),
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
			return true;
		}

		public async Task<bool> DeleteAdoptionFormAsync(Guid formId)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms.FirstAsync(x => x.Id == formId);
			UnitOfWork.AdoptionForms.Delete(form);
			await UnitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<DetailAdoptionFormResponseModel> GetAdoptionFormAsync(Guid formId)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.AsTracking()
				.FirstOrDefaultAsync(x => x.Id == formId)
				?? throw new FormNotFoundException();
			if(UserContext.Role == UserRole.SystemAdmin)
			{
				form.IsSeenByAdmin = true;
			}
			form.IsSeen = true;
			UnitOfWork.AdoptionForms.Update(form);
			await UnitOfWork.SaveChangesAsync();
			return Mapper.Map<DetailAdoptionFormResponseModel>(form);
		}

		public async Task<List<AdoptionFormResponseModel>> GetAdoptionFormsByPetIdAsync(Guid petId)
		{
			List<AdoptionForm> forms = await UnitOfWork.AdoptionForms
				.Where(x => x.PetId == petId)
				.ToListAsync();
			List<AdoptionFormResponseModel> result = new();
			foreach(var form in forms)
			{
				string userName = await _userService.GetUserNameAsync(form.AdopterId);
				result.Append(new AdoptionFormResponseModel()
				{
					Id = form.Id,
					LastUpdatedAt = form.IsCreatedAt.CompareTo(form.IsUpdatedAt) > 0 ? form.IsCreatedAt : form.IsUpdatedAt,
					IsSeen = UserContext.Role == UserRole.SystemAdmin ? form.IsSeenByAdmin : form.IsSeen,
					Name = userName,
				});
			}
			result.OrderByDescending(x => x.LastUpdatedAt);
			return result;
		}

		public async Task<List<AdoptionFormResponseModel>> GetAdoptionFormsByUserIdAsync()
		{
			List<AdoptionForm> forms = await UnitOfWork.AdoptionForms
				.Where(x => x.AdopterId == UserContext.Id)
				.ToListAsync();
			List<AdoptionFormResponseModel> result = new();
			foreach (var form in forms)
			{
				string userName = await _userService.GetUserNameAsync(form.AdopterId);
				result.Append(new AdoptionFormResponseModel()
				{
					Id = form.Id,
					LastUpdatedAt = form.IsCreatedAt.CompareTo(form.IsUpdatedAt) > 0 ? form.IsCreatedAt : form.IsUpdatedAt,
					IsSeen = UserContext.Role == UserRole.SystemAdmin ? form.IsSeenByAdmin : form.IsSeen,
					Name = userName,
				});
			}
			result.OrderByDescending(x => x.LastUpdatedAt);
			return result;
		}

		public async Task<bool> PreCheckAsync(Guid petId)
		{
			bool isOwned = await UnitOfWork.Pets.AnyAsync(x => x.Id == petId && x.OwnerId == UserContext.Id);
			if(isOwned)
			{
				throw new OwnedPetException();
			}

			bool isAdopted = await UnitOfWork.AdoptionForms.AnyAsync(x => x.AdopterId == UserContext.Id && x.PetId == petId);
			if (isAdopted)
			{
				throw new AdoptedPetException();
			}

			return true;
		}

		public async Task<bool> UpdateAdoptionFormAsync(UpdateAdoptionRequestModel request)
		{
			AdoptionForm form = await UnitOfWork.AdoptionForms
				.AsTracking()
				.Where(x => x.Id == request.AdoptionFormId)
				.FirstAsync();

			form.HouseType = request.HouseType;
			form.DelayDuration = request.AdoptTime;
			form.Message = request.Message;
			form.IsUpdatedAt = DateTimeOffset.Now;
			form.IsOwnerBefore = request.IsOwnerBefore;
			form.IsSeen = false;
			form.IsSeenByAdmin = false;
			await UnitOfWork.SaveChangesAsync();
			return true;
		}
	}
}