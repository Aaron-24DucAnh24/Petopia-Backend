using FluentValidation;
using Petopia.Business.Models.Pet;

namespace Petopia.Business.Validators
{
  public class CreatePetRequestValidator : AbstractValidator<CreatePetRequestModel>
  {
    public CreatePetRequestValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required");

      RuleFor(x => x.Breed)
        .NotEmpty()
        .WithMessage("Breed is required");

      RuleFor(x => x.Images)
        .Must(x => x.Count > 0)
        .WithMessage("Images are required");

      RuleFor(x => x.Description)
        .NotEmpty()
        .WithMessage("Description is required");
    }
  }

  public class UpdatePetRequestValidator : AbstractValidator<UpdatePetRequestModel>
  {
    public UpdatePetRequestValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required");

      RuleFor(x => x.Breed)
        .NotEmpty()
        .WithMessage("Breed is required");

      RuleFor(x => x.Images)
        .Must(x => x.Count > 0)
        .WithMessage("Images are required");
    }
  }
}