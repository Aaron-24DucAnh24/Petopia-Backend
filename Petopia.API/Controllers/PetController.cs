using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;
using Petopia.Business.Models.Pet;
using Petopia.Business.Utils;

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

    [HttpPost("")]
    [Authorize]
    public async Task<ActionResult<CreatePetResponseModel>> CreatePet([FromBody] CreatePetRequestModel request)
    {
      return ResponseUtils.OkResult(await _petService.CreatePetAsync(request));
    }

    [HttpPost("")]
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

    [HttpPut("")]
    [Authorize]
    public async Task<ActionResult<UpdatePetResponseModel>> UpdatePet([FromBody] UpdatePetRequestModel request)
    {
      UpdatePetResponseModel result = await _petService.UpdatePetAsync(request);
      // TODO: background service to sync media records
      return ResponseUtils.OkResult(result);
    }

    [HttpDelete("{petId}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeletePet(Guid petId)
    {
      return ResponseUtils.OkResult(await _petService.DeletePetAsync(petId));
    }
  }
}
