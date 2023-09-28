using FluentValidation;
using Petopia.Business.Models.User;

namespace Petopia.Business.Validators
{
  public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
  {
    public ResetPasswordRequestValidator()
    {
      RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Password is required")
        .MinimumLength(8)
        .WithMessage("Minimum length of password is 8");
    }
  }
}