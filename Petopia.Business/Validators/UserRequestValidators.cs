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
      RuleFor(x => x.ProvinceCode).NotEmpty();
      RuleFor(x => x.DistrictCode).NotEmpty();
      RuleFor(x => x.WardCode).NotEmpty();

      RuleFor(x => x.FirstName).NotEmpty();
      RuleFor(x => x.LastName).NotEmpty();
    }
  }

  public class UpdateOrganizationValidator : AbstractValidator<UpdateOrganizationRequestModel>
  {
    public UpdateOrganizationValidator()
    {
      RuleFor(x => x.Phone).NotEmpty();
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.ProvinceCode).NotEmpty();
      RuleFor(x => x.DistrictCode).NotEmpty();
      RuleFor(x => x.WardCode).NotEmpty();

      RuleFor(x => x.Website).NotEmpty();
      RuleFor(x => x.OrganizationName).NotEmpty();
      RuleFor(x => x.Description).NotEmpty();

    }
  }

  public class UpgradeAccountValidator : AbstractValidator<UpgradeAccountRequestModel>
  {
    public UpgradeAccountValidator()
    {
      RuleFor(x => x.EntityName).NotEmpty();
      RuleFor(x => x.OrganizationName).NotEmpty();
      RuleFor(x => x.Phone).NotEmpty();
      RuleFor(x => x.Street).NotEmpty();
      RuleFor(x => x.Website).NotEmpty();
      RuleFor(x => x.Description).NotEmpty();
      RuleFor(x => x.TaxCode).NotEmpty();
      RuleFor(x => x.ProvinceCode).NotEmpty();
      RuleFor(x => x.DistrictCode).NotEmpty();
      RuleFor(x => x.WardCode).NotEmpty();
      RuleFor(x => x.Type).NotEmpty();
    }
  }

  public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
  {
    public ChangePasswordRequestValidator()
    {
      RuleFor(x => x.NewPassword)
        .NotEmpty()
        .MinimumLength(8);
    }
  }

  public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
  {
    public ResetPasswordRequestValidator()
    {
      RuleFor(x => x.Password)
        .NotEmpty()
        .MinimumLength(8);
    }
  }
}