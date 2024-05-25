using FluentValidation;

namespace LXP.Common.DTO
{
    public class QuestionOptionViewModelValidator : AbstractValidator<QuestionOptionViewModel>
    {
        public QuestionOptionViewModelValidator()
        {
            RuleFor(option => option.Option)
                .NotEmpty().WithMessage("Option text is required.");

            RuleFor(option => option.IsCorrect)
                .NotNull().WithMessage("IsCorrect must be provided.");
        }
    }
}