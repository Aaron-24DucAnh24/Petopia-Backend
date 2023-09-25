using FluentValidation;
using PetAdoption.Business.Models.Authentication;

namespace PetAdoption.Business.Validators
{
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
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

      RuleFor(x => x.ConfirmPassword)
        .Equal(x => x.Password)
        .WithMessage("Confirm password is not correct");
    }
  }
}