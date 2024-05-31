using FluentValidation;
using LXP.Common.ViewModels;

public class CourseCategoryViewModelValidator : AbstractValidator<CourseCategoryViewModel>
{
    public CourseCategoryViewModelValidator()
    {
        RuleFor(courseCategory => courseCategory.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(courseCategory => courseCategory.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required");
    }
}