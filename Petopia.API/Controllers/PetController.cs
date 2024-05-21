using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Pet;
using Petopia.Business.Utils;
using Petopia.Data.Enums;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/Pet")]
  public class PetController : ControllerBase
  {
    private readonly IPetService _petService;

    public PetController(
      IPetService petService
    )
    {
      _petService = petService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreatePetResponseModel>> CreatePet([FromBody] CreatePetRequestModel request)
    {
      return ResponseUtils.OkResult(await _petService.CreatePetAsync(request));
    }

    [HttpPost("Get")]
    [AllowAnonymous]
    public async Task<ActionResult<PaginationResponseModel<PetResponseModel>>> GetPets([FromBody] PaginationRequestModel<PetFilterModel> request)
    {
      return ResponseUtils.OkResult(await _petService.GetPetsAsync(request));
    }

    [HttpGet("{petId}/Details")]
    [AllowAnonymous]
    public async Task<ActionResult<PetDetailsResponseModel>> GetPetDetails(Guid petId)
    {
      return ResponseUtils.OkResult(await _petService.GetPetDetailsAsync(petId));
    }

    [HttpPost("User")]
    [AllowAnonymous]
    public async Task<ActionResult<PaginationResponseModel<PetResponseModel>>> GetPetsByUserId([FromBody] PaginationRequestModel<Guid> request)
    {
      return ResponseUtils.OkResult(await _petService.GetPetsByUserId(request));
    }

    [HttpPut("")]
    [Authorize]
    public async Task<ActionResult<UpdatePetResponseModel>> UpdatePet([FromBody] UpdatePetRequestModel request)
    {
      UpdatePetResponseModel result = await _petService.UpdatePetAsync(request);
      return ResponseUtils.OkResult(result);
    }

    [HttpDelete("{petId}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeletePet(Guid petId)
    {
      return ResponseUtils.OkResult(await _petService.DeletePetAsync(petId));
    }

    [HttpGet("Breed")]
    [Authorize]
    public async Task<ActionResult<List<string>>> GetBreeds([FromQuery] PetSpecies species)
    {
      return ResponseUtils.OkResult(await _petService.GetBreedsAsync(species));
    }

    [HttpGet("AvailableBreed")]
    [AllowAnonymous]
    public async Task<ActionResult<List<string>>> GetAvailableBreeds([FromQuery] PetSpecies species)
    {
      return ResponseUtils.OkResult(await _petService.GetAvailableBreedsAsync(species));
    }

    [HttpGet("Keywords")]
    [AllowAnonymous]
    public async Task<ActionResult<List<string>>> GetKeywords()
    {
      return ResponseUtils.OkResult(await _petService.GetKeywordsAsync());
    }
  }
}
