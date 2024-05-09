using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Adoption;
using Petopia.Business.Utils;
using Petopia.Data.Enums;

namespace Petopia.API.Controllers
{
  [ApiController]
  [Route("api/AdoptionForm")]
  public class AdoptionFormController : ControllerBase
  {
    private readonly IAdoptionFormService _adoptionFormService;

    public AdoptionFormController(IAdoptionFormService adoptionFormService)
    {
      _adoptionFormService = adoptionFormService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<bool>> CreateAdoption([FromBody] CreateAdoptionRequestModel request)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.CreateAdoptionFormAsync(request));
    }

    [HttpGet("PreCheck/{petId}")]
    [Authorize]
    public async Task<ActionResult<bool>> PreCheck(Guid petId)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.PreCheckAsync(petId));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<DetailAdoptionFormResponseModel>> GetAdoptionForm(Guid id)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.GetAdoptionFormAsync(id));
    }

    [HttpGet("Incoming")]
    [Authorize]
    public async Task<ActionResult<List<AdoptionFormResponseModel>>> GetAdoptionFormsByPetId()
    {
      return ResponseUtils.OkResult(await _adoptionFormService.GetAdoptionFormsIncomingAsync());
    }

    [HttpGet("CountUnreadIncoming")]
    [Authorize]
    public async Task<ActionResult<int>> CountUnreadIncommingRequest()
    {
      return ResponseUtils.OkResult(await _adoptionFormService.CountUnreadRequestByUserAsync());
    }

    [HttpGet("Sent")]
    [Authorize]
    public async Task<ActionResult<List<AdoptionFormResponseModel>>> GetAdoptionFormsByUserId()
    {
      return ResponseUtils.OkResult(await _adoptionFormService.GetAdoptionFormsByUserIdAsync());
    }

    [HttpPut("{id}/Accept")]
    [Authorize]
    public async Task<ActionResult<bool>> AcceptAdoptionForm(Guid id)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.ActOnAdoptionFormAsync(id, AdoptStatus.Accepted));
    }

    [HttpPut("{id}/Reject")]
    [Authorize]
    public async Task<ActionResult<bool>> RejectAdoptionForm(Guid id)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.ActOnAdoptionFormAsync(id, AdoptStatus.Rejected));
    }

    [HttpPut("{id}/Cancel")]
    [Authorize]
    public async Task<ActionResult<bool>> CancelAdoptionForm(Guid id)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.DeleteAdoptionFormAsync(id));
    }

    [HttpPut("{id}/Confirm")]
    [Authorize]
    public async Task<ActionResult<bool>> ConfirmAdoptionForm(Guid id)
    {
      return ResponseUtils.OkResult(await _adoptionFormService.ActOnAdoptionFormAsync(id, AdoptStatus.Adopted));
    }
  }
}