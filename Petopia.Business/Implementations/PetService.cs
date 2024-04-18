using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Pet;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class PetService : BaseService, IPetService
  {
    private readonly IElasticsearchService _elasticsearchService;
    private const int SEE_MORE_LENGTH = 4;

    public PetService(
      IServiceProvider provider,
      ILogger<PetService> logger,
      IElasticsearchService elasticsearchService
    ) : base(provider, logger)
    {
      _elasticsearchService = elasticsearchService;
    }

    public async Task<bool> DeletePetAsync(Guid petId)
    {
      Pet pet = await UnitOfWork.Pets
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == petId && x.OwnerId == UserContext.Id)
        ?? throw new PetNotFoundException();

      pet.IsDeleted = true;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();

      return true;
    }

    public async Task<CreatePetResponseModel> CreatePetAsync(CreatePetRequestModel model)
    {
      Pet pet = await UnitOfWork.Pets.CreateAsync(new Pet()
      {
        Id = Guid.NewGuid(),
        Name = model.Name,
        Description = model.Description,
        Sex = model.Sex,
        Age = model.Age,
        Color = model.Color,
        Species = model.Species,
        Size = model.Size,
        Breed = model.Breed,
        IsVaccinated = model.IsVaccinated,
        IsSterillized = model.IsSterillized,
        IsAvailable = model.IsAvailable,
        OwnerId = UserContext.Id,
        IsCreatedAt = DateTimeOffset.Now,
      });

      foreach (var image in model.Images)
      {
        UnitOfWork.Medias.Create(new Media()
        {
          Id = Guid.NewGuid(),
          PetId = pet.Id,
          Url = image,
          Type = MediaType.Image,
        });
      }

      await UnitOfWork.SaveChangesAsync();
      var result = Mapper.Map<CreatePetResponseModel>(model);
      result.Images = model.Images;
      result.Id = pet.Id;
      return result;
    }

    public async Task<PetDetailsResponseModel> GetPetDetailsAsync(Guid petId)
    {
      Pet pet = await UnitOfWork.Pets
        .AsTracking()
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Where(x => x.Id == petId)
        .FirstOrDefaultAsync()
        ?? throw new PetNotFoundException();

      pet.View += 1;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();

      List<Pet> seeMore = await UnitOfWork.Pets
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Where(x => x.Species == pet.Species && x.Color == pet.Color && x.Id != pet.Id)
        .ToListAsync();
      if(seeMore.Count == 0)
      {
				seeMore = await UnitOfWork.Pets
				.Include(x => x.Images)
        .Where(x => x.Id != pet.Id)
        .Take(SEE_MORE_LENGTH)
				.ToListAsync();
			}

      var result = Mapper.Map<PetDetailsResponseModel>(pet);
      result.Address = pet.Owner.Address;
      result.SeeMore = Mapper.Map<List<PetResponseModel>>(seeMore);

      return result;
    }

    public async Task<PaginationResponseModel<PetResponseModel>> GetPetsAsync(PaginationRequestModel<PetFilterModel> model)
    {
      IQueryable<Pet> query = UnitOfWork.Pets
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Where(x => !x.IsDeleted)
        .AsQueryable();

      query = GetPetsFromFilter(query, model.Filter);
      query = GetPetsFromText(query, model.Filter.Text);
      
			if (!string.IsNullOrEmpty(model.OrderBy))
			{
				query = model.OrderBy == OrderKey.NEWEST
				? query.OrderByDescending(x => x.IsCreatedAt)
				: query.OrderByDescending(x => x.View);
			}
			return await PagingAsync<PetResponseModel, Pet>(query, model);
    }

    public async Task<UpdatePetResponseModel> UpdatePetAsync(UpdatePetRequestModel model)
    {
      Pet pet = await UnitOfWork.Pets
        .AsTracking()
        .FirstAsync(x => x.Id == model.Id);

      pet.Name = model.Name;
      pet.Description = model.Description;
      pet.Sex = model.Sex;
      pet.Age = model.Age;
      pet.Color = model.Color;
      pet.Species = model.Species;
      pet.Size = model.Size;
      pet.Breed = model.Breed;
      pet.IsVaccinated = model.IsVaccinated;
      pet.IsSterillized = model.IsSterillized;
      pet.IsAvailable = model.IsAvailable;
      pet.IsUpdatedAt = DateTimeOffset.Now;

      List<Media> images = await UnitOfWork.Medias
        .AsTracking()
        .Where(x => x.PetId == model.Id)
        .ToListAsync();

      foreach (var image in images)
      {
        if(!model.Images.Contains(image.Url))
        {
          UnitOfWork.Medias.Delete(image);
        }
      }

      foreach (var image in model.Images)
      {
        if(!images.Select(x => x.Url).ToList().Contains(image))
        {
					UnitOfWork.Medias.Create(new Media()
					{
						Id = Guid.NewGuid(),
						PetId = pet.Id,
						Url = image,
						Type = MediaType.Image,
					});
				}
      }

      await UnitOfWork.SaveChangesAsync();
      var result = Mapper.Map<UpdatePetResponseModel>(model);
      result.Images = model.Images;
      return result;
    }

		public async Task<PaginationResponseModel<PetResponseModel>> GetPetsByUserId(PaginationRequestModel<Guid> model)
		{
			IQueryable<Pet> query = UnitOfWork.Pets
	      .Include(x => x.Images)
        .Include(x => x.Owner)
	      .Where(x => !x.IsDeleted)
        .Where(x => x.OwnerId == model.Filter)
	      .AsQueryable();
			return await PagingAsync<PetResponseModel, Pet>(query, model);
		}

		#region private

		private IQueryable<Pet> GetPetsFromText(IQueryable<Pet> query, string? keyword)
    {
      return query;
    }

    private IQueryable<Pet> GetPetsFromFilter(IQueryable<Pet> query, PetFilterModel filter)
    {
      if (filter.Age != null && filter.Age.Any())
      {
        query = query.Where(x => filter.Age.Contains(x.Age));
      }
      if (filter.Color != null && filter.Color.Any())
      {
        query = query.Where(x => filter.Color.Contains(x.Color));
      }
      if (filter.IsSterillized != null && filter.IsSterillized.Any())
      {
        query = query.Where(x => filter.IsSterillized.Contains(x.IsSterillized));
      }
      if (filter.IsVaccinated != null && filter.IsVaccinated.Any())
      {
        query = query.Where(x => filter.IsVaccinated.Contains(x.IsVaccinated));
      }
      if (filter.Sex != null && filter.Sex.Any())
      {
        query = query.Where(x => filter.Sex.Contains(x.Sex));
      }
      if (filter.Size != null && filter.Size.Any())
      {
        query = query.Where(x => filter.Size.Contains(x.Size));
      }
      if (filter.Species != null && filter.Species.Any())
      {
        query = query.Where(x => filter.Species.Contains(x.Species));
      }
      return query;
    }

		#endregion
	}
}
