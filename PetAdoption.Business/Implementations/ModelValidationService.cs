using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class ModelValidationService : IModelValidationService
  {
    private readonly IServiceProvider _provider;

    public ModelValidationService(IServiceProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> ValidateAsync<T>(T obj, ModelStateDictionary modelState)
    {
      var validationResult = await _provider.GetRequiredService<IValidator<T>>().ValidateAsync(obj);
      if (validationResult.IsValid)
      {
        return true;
      }
      validationResult.AddToModelState(modelState);
      return false;
    }
  }
}