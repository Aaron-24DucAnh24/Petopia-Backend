using FluentValidation;
using Petopia.Business.Models.User;

namespace Petopia.Business.Validators
{
  public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
  {
    public ChangePasswordRequestValidator()
    {
      RuleFor(x => x.NewPassword)
        .NotEmpty()
        .WithMessage("Password is required")
        .MinimumLength(8)
        .WithMessage("Minimum length of password is 8");
    }
  }
}