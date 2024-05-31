using FluentValidation;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Http;

public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
{
    public CourseViewModelValidator()
    {
        RuleFor(course => course.Title)
            .NotEmpty().WithMessage("Title is required");

        RuleFor(course => course.Level)
            .NotEmpty().WithMessage("Level is required");

        RuleFor(course => course.Catagory)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(course => course.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(course => course.Duration)
             .GreaterThan(0).WithMessage("Duration must be greater than 0")
             .Must(BeInDecimalForm).WithMessage("Duration must be in decimal form");

        RuleFor(course => course.Thumbnailimage)
            .Must(BeAValidSize).WithMessage("Image must be less than 250kb")
            .Must(BeAValidFileType).WithMessage("File must be jpeg or png");
    }

    private bool BeAValidSize(IFormFile file)
    {
        // Assuming size is in bytes
        return file.Length < 250 * 1024;
    }

    private bool BeAValidFileType(IFormFile file)
    {
        var validFileTypes = new[] { "image/jpeg", "image/png" };
        return validFileTypes.Contains(file.ContentType);
    }
    private bool BeInDecimalForm(decimal duration)
    {
        // Check if the duration has a fractional part
        return duration % 1 != 0;
    }
}