using FluentValidation;
using Petopia.Business.Models.Post;

namespace Petopia.Business.Validators
{
  public class CreatePostRequestValidator : AbstractValidator<CreatePostRequestModel>
  {
    public CreatePostRequestValidator()
    {
      RuleFor(x => x.Content).NotEmpty();
      RuleFor(x => x.Images)
        .Must(x => x.Count > 0)
        .WithMessage("Images are required");
    }
  }
}