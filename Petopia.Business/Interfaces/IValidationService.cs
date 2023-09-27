using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Petopia.Business.Interfaces
{
  public interface IValidationService
  {
    public Task<bool> ValidateAsync<T>(T obj, ModelStateDictionary modelState);
  }
}