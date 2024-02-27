using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.AdoptionForm;
using Petopia.Business.Utils;
using Petopia.Data.Enums;
using System.Text.Json;

namespace Petopia.API.Controllers
{
    [ApiController]
    [Route("api/AdaptionForm")]
    public class AdaptionFormController : ControllerBase
    {
        private readonly IAdoptionFormService _adoptionFormService;

        public AdaptionFormController(IAdoptionFormService adoptionFormService)
        {
            _adoptionFormService = adoptionFormService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionFormDataModel>> GetAdoptionFormById(Guid id)
        {
            return ResponseUtils.OkResult(await _adoptionFormService.GetAdoptionFormByIdAsync(id));
        }

        [HttpGet("Pet/{petId}")]
        public async Task<ActionResult<AdoptionFormDataModel>> GetAdoptionFormByPetId(Guid petId)
        {
            return ResponseUtils.OkResult(await _adoptionFormService.GetAdoptionFormByPetIdAsync(petId));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateAdoptionForm()
        {
            await _adoptionFormService.CreateAdoptionFormAsync();
            return ResponseUtils.OkResult(true);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateAdoptionForm( [FromBody] AdoptionFormDataModel data )
        {
            //JSON Format:
            
/*            var data = JsonSerializer.Deserialize<AdoptionFormDataModel>(jsonElement.ToString());
            if (data == null)
            {
                return BadRequest();
            }*/
            await _adoptionFormService.UpdateAdoptionFormAsync( data);
            return ResponseUtils.OkResult(true);
        }

        [HttpPut("{id}/Accept")]
        public async Task<ActionResult<bool>> AcceptAdoptionForm(Guid id)
        {
            await _adoptionFormService.HandleAdoptionFormAsync(id, AdoptStatus.Accepted);
            return ResponseUtils.OkResult(true);
        }

        [HttpPut("{id}/Reject")]
        public async Task<ActionResult<bool>> RejectAdoptionForm(Guid id)
        {
            await _adoptionFormService.HandleAdoptionFormAsync(id, AdoptStatus.Rejected);
            return ResponseUtils.OkResult(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAdoptionForm(Guid id)
        {
            await _adoptionFormService.DeleteAdoptionFormAsync(id);
            return ResponseUtils.OkResult(true);
        }
    }
}