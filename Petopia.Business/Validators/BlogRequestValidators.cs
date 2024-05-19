using FluentValidation;
using Petopia.Business.Models.Blog;

namespace Petopia.Business.Validators
{
  public class CreateBlogRequestValidator : AbstractValidator<CreateBlogRequestModel>
  {
    public CreateBlogRequestValidator()
    {
      RuleFor(x => x.Image)
        .NotEmpty()
        .WithMessage("Image is required");

      RuleFor(x => x.Title)
        .NotEmpty()
        .WithMessage("Title is required");

      RuleFor(x => x.Excerpt)
        .NotEmpty()
        .WithMessage("Excerpt is required");

      RuleFor(x => x.Content)
        .NotEmpty()
        .WithMessage("Content is required");
    }
  }

  public class UpdateBlogRequestValidator : AbstractValidator<UpdateBlogRequestModel>
  {
    public UpdateBlogRequestValidator()
    {
      RuleFor(x => x.Image)
        .NotEmpty()
        .WithMessage("Image is required");

      RuleFor(x => x.Title)
        .NotEmpty()
        .WithMessage("Title is required");

      RuleFor(x => x.Excerpt)
        .NotEmpty()
        .WithMessage("Excerpt is required");

      RuleFor(x => x.Content)
        .NotEmpty()
        .WithMessage("Content is required");
    }
  }
}