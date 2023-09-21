using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetAdoption.Business.Interfaces
{
  public interface IModelValidationService
  {
    public Task<bool> ValidateAsync<T>(T obj, ModelStateDictionary modelState);
  }
}