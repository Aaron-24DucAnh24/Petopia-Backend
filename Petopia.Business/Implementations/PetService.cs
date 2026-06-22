using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    private const double BREED_CACHING_DAYS = Constants.CACHE_TIME_LOCATION;
    private const string NAMES_CACHE_KEY = "NAMES_CACHE_KEY";
    private const string BREEDS_CACHE_KEY = "BREEDS_CACHE_KEY";
    private const string DOG_KEYWORD = "Chó";
    private const string CAT_KEYWORD = "Mèo";

    private readonly ISearchEngineService _searchEngineService;

    public PetService(
      IServiceProvider provider,
      ILogger<PetService> logger
    ) : base(provider, logger)
    {
      _searchEngineService = provider.GetRequiredService<ISearchEngineService>();
    }

    public async Task<bool> DeletePetAsync(Guid petId)
    {
      var pet = await UnitOfWork.Pets
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Id == petId && x.OwnerId == UserContext.Id)
        ?? throw new PetNotFoundException();

      pet.IsDeleted = true;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();

      await _searchEngineService.DeleteAsync(Constants.MEILISEARCH_INDEX_PET, pet.Id.ToString());

      return true;
    }

    public async Task<CreatePetResponseModel> CreatePetAsync(CreatePetRequestModel model)
    {
      var pet = await UnitOfWork.Pets.CreateAsync(new Pet
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
        UnitOfWork.Medias.Create(new Media
        {
          Id = Guid.NewGuid(),
          PetId = pet.Id,
          Url = image,
          Type = MediaType.Image,
        });
      }

      foreach (var vaccineId in model.VaccineIds)
      {
        UnitOfWork.PetVaccines.Create(new PetVaccine
        {
          PetId = pet.Id,
          VaccineId = vaccineId,
        });
      }

      await UnitOfWork.SaveChangesAsync();

      var cacheKey = GetBreedCacheKey(model.Species, isAvalable: true);
      CacheManager.Instance.Remove(cacheKey);

      var createdPet = await UnitOfWork.Pets
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .FirstOrDefaultAsync(x => x.Id == pet.Id);
      if (createdPet is not null)
      {
        await _searchEngineService.InsertUpdateAsync(
          Constants.MEILISEARCH_INDEX_PET,
          Mapper.Map<PetSearchModel>(createdPet));
      }

      var result = Mapper.Map<CreatePetResponseModel>(model);
      result.Images = model.Images;
      result.Id = pet.Id;

      return result;
    }

    public async Task<PetDetailsResponseModel> GetPetDetailsAsync(Guid petId)
    {
      var pet = await UnitOfWork.Pets
        .AsTracking()
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Include(x => x.PetVaccines)
        .ThenInclude(x => x.Vaccine)
        .AsSplitQuery()
        .Where(x => x.Id == petId)
        .FirstOrDefaultAsync()
        ?? throw new PetNotFoundException();

      pet.View += 1;
      UnitOfWork.Pets.Update(pet);
      await UnitOfWork.SaveChangesAsync();

      var seeMore = await UnitOfWork.Pets
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Where(x => (x.Species == pet.Species)
          && (x.Color == pet.Color)
          && (x.Id != pet.Id)
          && (!x.IsDeleted))
        .OrderByDescending(x => x.IsCreatedAt)
        .Take(SEE_MORE_LENGTH)
        .ToListAsync();

      if (seeMore.Count == 0)
      {
        seeMore = await UnitOfWork.Pets
        .Include(x => x.Images)
        .Where(x => x.Id != pet.Id)
        .OrderByDescending(x => x.IsCreatedAt)
        .Take(SEE_MORE_LENGTH)
        .ToListAsync();
      }

      var result = Mapper.Map<PetDetailsResponseModel>(pet);
      result.Address = pet.Owner.Address;
      result.SeeMore = Mapper.Map<List<PetResponseModel>>(seeMore);
      result.Vaccines = pet.PetVaccines.Select(x => new VaccineResponseModel
      {
        Name = x.Vaccine.Name,
        Id = x.Vaccine.Id
      }).ToList();

      return result;
    }

    public async Task<PaginationResponseModel<PetResponseModel>> GetPetsAsync(PaginationRequestModel<PetFilterModel> model)
    {
      var result = await _searchEngineService.SearchAsync<PetResponseModel, PetFilterModel, PetSearchModel>(Constants.MEILISEARCH_INDEX_PET, model);
      return result;
    }

    public async Task<UpdatePetResponseModel> UpdatePetAsync(UpdatePetRequestModel model)
    {
      var pet = await UnitOfWork.Pets
        .AsTracking()
        .Include(x => x.PetVaccines)
        .Include(x => x.Images)
        .Include(x => x.Owner)
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

      var images = await UnitOfWork.Medias
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
          UnitOfWork.Medias.Create(new Media
          {
            Id = Guid.NewGuid(),
            PetId = pet.Id,
            Url = image,
            Type = MediaType.Image,
          });
        }
      }

      foreach (var vaccineId in model.VaccineIds)
      {
        if (!pet.PetVaccines.Any(x => x.VaccineId == vaccineId))
        {
          UnitOfWork.PetVaccines.Create(new PetVaccine
          {
            PetId = pet.Id,
            VaccineId = vaccineId,
          });
        }
      }

      foreach (var petVaccine in pet.PetVaccines)
      {
        if (!model.VaccineIds.Contains(petVaccine.VaccineId))
        {
          UnitOfWork.PetVaccines.Delete(petVaccine);
        }
      }

      await UnitOfWork.SaveChangesAsync();

      var cacheKey = GetBreedCacheKey(model.Species, isAvalable: true);
      CacheManager.Instance.Remove(cacheKey);

      await _searchEngineService.InsertUpdateAsync(Constants.MEILISEARCH_INDEX_PET, Mapper.Map<PetSearchModel>(pet));

      var result = Mapper.Map<UpdatePetResponseModel>(model);
      result.Images = model.Images;

      return result;
    }

    public async Task<PaginationResponseModel<PetResponseModel>> GetPetsByUserId(PaginationRequestModel<Guid> model)
    {
      var query = UnitOfWork.Pets
        .Include(x => x.Images)
        .Include(x => x.Owner)
        .Where(x => !x.IsDeleted)
        .Where(x => x.OwnerId == model.Filter)
        .AsQueryable();
      var result = await PagingAsync<PetResponseModel, Pet>(query, model);

      return result;
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
        BREED_CACHING_DAYS) ?? new List<string>();

      return result;
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
        BREED_CACHING_DAYS) ?? new List<string>();
      result = result.Order().ToList();

      return result;
    }

    public async Task<List<string>> GetKeywordsAsync()
    {
      var nameQuery = UnitOfWork.Pets
        .Where(x => !x.IsDeleted)
        .Select(x => x.Name)
        .Distinct()
        .AsQueryable();
      var names = await CacheManager.Instance.GetOrSetAsync(
        nameQuery,
        NAMES_CACHE_KEY,
        BREED_CACHING_DAYS);
      var breedQuery = UnitOfWork.Pets
        .Where(x => !x.IsDeleted)
        .Select(x => x.Breed)
        .Distinct()
        .AsQueryable();
      var breeds = await CacheManager.Instance.GetOrSetAsync(
        breedQuery,
        BREEDS_CACHE_KEY,
        BREED_CACHING_DAYS);
      var result = new List<string>();

      if (names is not null && breeds is not null)
      {
        names.Add(DOG_KEYWORD);
        names.Add(CAT_KEYWORD);
        result.AddRange(names.Concat(breeds));
      }

      return result;
    }

    public async Task<List<VaccineResponseModel>> GetVaccinesAsync()
    {
      var vaccines = await UnitOfWork.Vaccines.ToListAsync();
      var result = Mapper.Map<List<VaccineResponseModel>>(vaccines);

      return result;
    }

    #region private
    private static string GetBreedCacheKey(PetSpecies species, bool isAvalable = false)
    {
      var result = species == PetSpecies.Dog ? "DogBreeds" : "CatBreeds";
      if (isAvalable)
      {
        return result + "Available";
      }

      return result;
    }
    #endregion
  }
}
