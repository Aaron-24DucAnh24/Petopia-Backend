using FluentValidation;
using Petopia.Business.Models.Authentication;

namespace Petopia.Business.Validators
{
  public class RegisterRequestValidator : AbstractValidator<RegisterRequestModel>
  {
    public RegisterRequestValidator()
    {
      RuleFor(x => x.Email)
        .NotEmpty()
        .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

      RuleFor(x => x.Password)
        .NotEmpty()
        .MinimumLength(8)
        .MaximumLength(128);

      RuleFor(x => x.FirstName)
        .NotEmpty()
        .MaximumLength(128);

      RuleFor(x => x.LastName)
        .NotEmpty()
        .MaximumLength(128);

      RuleFor(x => x.BirthDate)
        .NotEmpty()
        .Must(x => DateTimeOffset.TryParse(x, out DateTimeOffset dateTime));
    }
  }
}