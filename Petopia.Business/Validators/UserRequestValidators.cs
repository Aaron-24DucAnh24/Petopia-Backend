using FluentValidation;
using Petopia.Business.Models.User;

namespace Petopia.Business.Validators
{
  public class UpdateUserValidator : AbstractValidator<UpdateUserRequestModel>
  {
    public UpdateUserValidator()
    {
      RuleFor(x => x.Phone).NotEmpty();
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.FirstName).NotEmpty();
      RuleFor(x => x.LastName).NotEmpty();
      RuleFor(x => x.Description).NotEmpty();
    }
  }

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

  public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
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