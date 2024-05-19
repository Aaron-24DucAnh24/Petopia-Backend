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
        .WithMessage("Mail is required")
        .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
        .WithMessage("Invalid mail format");

      RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Password is required")
        .MinimumLength(8)
        .WithMessage("Minimum length of password is 8");
    }
  }
}