using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetAdoption.Business.Interfaces
{
  public interface IValidationService
  {
    public Task<bool> ValidateAsync<T>(T obj, ModelStateDictionary modelState);
  }
}