﻿using Microsoft.EntityFrameworkCore;
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
      User userAttributes = await GetUserAttributesAsync();
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
        Address = string.IsNullOrEmpty(model.Address) ? userAttributes.Address : model.Address,
        OwnerId = UserContext.Id,
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

    public Task<PetDetailsResponseModel> GetPetDetailsAsync(Guid petId)
    {
      throw new NotImplementedException();
    }

    public async Task<PaginationResponseModel<PetResponseModel>> GetPetsAsync(PaginationRequestModel<PetFilterModel> model)
    {
      var query = UnitOfWork.Pets.AsQueryable();
      var filter = model.Filter;

      if (filter.Age != null)
      {
        query = query.Where(x => filter.Age.Contains(x.Age));
      }
      if (filter.Color != null)
      {
        query = query.Where(x => filter.Color.Contains(x.Color));
      }
      if (filter.IsSterillized != null)
      {
        query = query.Where(x => filter.IsSterillized.Contains(x.IsSterillized));
      }
      if (filter.IsVaccinated != null)
      {
        query = query.Where(x => filter.IsVaccinated.Contains(x.IsVaccinated));
      }
      if (filter.Sex != null)
      {
        query = query.Where(x => filter.Sex.Contains(x.Sex));
      }
      if (filter.Size != null)
      {
        query = query.Where(x => filter.Size.Contains(x.Size));
      }
      if (filter.Species != null)
      {
        query = query.Where(x => filter.Species.Contains(x.Species));
      }
      if (string.IsNullOrEmpty(model.OrderBy))
      {
        query = model.OrderBy == OrderKey.NEWEST
        ? query.OrderByDescending(x => x.IsCreatedAt)
        : query.OrderByDescending(x => x.View);
      }

      return await PagingAsync<PetResponseModel, Pet, PetFilterModel>(query, model);
    }

    public async Task<UpdatePetResponseModel> UpdatePetAsync(UpdatePetRequestModel model)
    {
      User userAttributes = await GetUserAttributesAsync();

      Pet pet = await UnitOfWork.Pets.AsTracking().FirstAsync(x => x.Id == model.Id);
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
      pet.Address = string.IsNullOrEmpty(model.Address) ? userAttributes.Address : model.Address;

      var images = await UnitOfWork.Medias.AsTracking().Where(x => x.PetId == model.Id).ToListAsync();
      foreach (var image in images)
      {
        image.IsDeleted = true;
      }

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
      var result = Mapper.Map<UpdatePetResponseModel>(model);
      result.Images = model.Images;
      return result;
    }
  }
}