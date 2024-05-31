using FluentValidation;
using LXP.Common.ViewModels;

public class CourseTopicViewModelValidator : AbstractValidator<CourseTopicViewModel>
{
    public CourseTopicViewModelValidator()
    {
        RuleFor(courseTopic => courseTopic.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(courseTopic => courseTopic.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(courseTopic => courseTopic.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(courseTopic => courseTopic.CreatedBy)
            .NotEmpty().WithMessage("Created By is required");
    }
}