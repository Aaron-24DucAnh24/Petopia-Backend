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
    private const int SEE_MORE_LENGTH = 4;
    private const double BREED_CACHING_DAYS = (double)30 / 60 / 24;
    private const string NAMES_CACHE_KEY = "NAMES_CACHE_KEY";
    private const string BREEDS_CACHE_KEY = "BREEDS_CACHE_KEY";
    private const string DOG_KEYWORD = "Chó";
    private const string CAT_KEYWORD = "Mèo";

    public PetService(IServiceProvider provider, ILogger<PetService> logger) : base(provider, logger)
    { }

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

      CacheManager.Instance.Remove(GetBreedCacheKey(model.Species, true));
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
        .Where(x => x.Species == pet.Species && x.Color == pet.Color && x.Id != pet.Id && !x.IsDeleted)
        .Take(SEE_MORE_LENGTH)
        .ToListAsync();

      if (seeMore.Count == 0)
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

      if (string.IsNullOrEmpty(model.Filter.Text))
      {
        query = GetPetsFromFilter(query, model.Filter);
      }
      else {
        query = GetPetsFromText(query, model.Filter.Text);
      }

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
        if (!model.Images.Contains(image.Url))
        {
          UnitOfWork.Medias.Delete(image);
        }
      }

      foreach (var image in model.Images)
      {
        if (!images.Select(x => x.Url).ToList().Contains(image))
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

      CacheManager.Instance.Remove(GetBreedCacheKey(model.Species, true));
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

    public async Task<List<string>> GetBreedsAsync(PetSpecies species)
    {
      var query = UnitOfWork.PetBreeds
        .Where(x => x.Species == species)
        .OrderBy(x => x.Name)
        .Select(x => x.Name)
        .AsQueryable();
      var result = await CacheManager.Instance.GetOrSetAsync(
        query,
        GetBreedCacheKey(species),
        BREED_CACHING_DAYS
      );
      if (result != null)
      {
        return result.ToList();
      }
      return new List<string>();
    }

    public async Task<List<string>> GetAvailableBreedsAsync(PetSpecies species)
    {
      var query = UnitOfWork.Pets
        .Where(x => !x.IsDeleted && x.Species == species)
        .Select(x => x.Breed)
        .Distinct()
        .AsQueryable();
      var result = await CacheManager.Instance.GetOrSetAsync(
        query,
        GetBreedCacheKey(species, true),
        BREED_CACHING_DAYS
      );
      if (result != null)
      {
        return result.Order().ToList();
      }
      return new List<string>();
    }

    public async Task<List<string>> GetKeywordsAsync()
    {
      var nameQuery = UnitOfWork.Pets
        .Where(x => !x.IsDeleted)
        .Select(x => x.Name)
        .Distinct()
        .AsQueryable();
      List<string>? names = await CacheManager.Instance.GetOrSetAsync(
        nameQuery,
        NAMES_CACHE_KEY,
        BREED_CACHING_DAYS
      );

      var breedQuery = UnitOfWork.Pets
        .Where(x => !x.IsDeleted)
        .Select(x => x.Breed)
        .Distinct()
        .AsQueryable();
      List<string>? breeds = await CacheManager.Instance.GetOrSetAsync(
        breedQuery,
        BREEDS_CACHE_KEY,
        BREED_CACHING_DAYS
      );

      if(names != null && breeds != null)
      {
        names.Add(DOG_KEYWORD);
        names.Add(CAT_KEYWORD);
        return names.Concat(breeds).ToList();
      }
      return new List<string>();
    }

    #region private

    private IQueryable<Pet> GetPetsFromText(IQueryable<Pet> query, string keyword)
    {
      if(keyword == DOG_KEYWORD)
      {
        return query.Where(x => x.Species == PetSpecies.Dog);
      }
      if(keyword == CAT_KEYWORD)
      {
        return query.Where(x => x.Species == PetSpecies.Cat);
      }
      return query.Where(x => x.Name == keyword || x.Breed == keyword);
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
      if (filter.Breed != null && filter.Breed.Any())
      {
        query = query.Where(x => filter.Breed.Contains(x.Breed));
      }
      return query;
    }

    private string GetBreedCacheKey(PetSpecies species, bool isAvalable = false)
    {
      string result = species == PetSpecies.Dog ? "DogBreeds" : "CatBreeds";
      if (isAvalable)
      {
        return result + "Available";
      }
      return result;
    }

    #endregion
  }
}
