using FluentValidation;
using Petopia.Business.Models.User;

namespace Petopia.Business.Validators
{
	public class UpdateUserValidator : AbstractValidator<UpdateUserRequestModel>
	{
		public UpdateUserValidator()
		{
			RuleFor(x => x.Phone).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.LastName).NotEmpty();
			RuleFor(x => x.ProvinceCode).NotEmpty();
			RuleFor(x => x.DistrictCode).NotEmpty();
			RuleFor(x => x.WardCode).NotEmpty();
			RuleFor(x => x.Street).NotEmpty();
		}
	}
}