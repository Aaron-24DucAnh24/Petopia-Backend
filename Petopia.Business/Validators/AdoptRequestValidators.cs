using FluentValidation;
using Petopia.Business.Models.Adoption;

namespace Petopia.Business.Validators
{
  public class CreateAdoptionRequestValidator : AbstractValidator<CreateAdoptionRequestModel>
  {
    public CreateAdoptionRequestValidator()
    {
      RuleFor(x => x.ProvinceCode).NotEmpty();
      RuleFor(x => x.DistrictCode).NotEmpty();
      RuleFor(x => x.WardCode).NotEmpty();
      RuleFor(x => x.Phone).NotEmpty();
      RuleFor(x => x.Street).NotEmpty();
    }
  }
}