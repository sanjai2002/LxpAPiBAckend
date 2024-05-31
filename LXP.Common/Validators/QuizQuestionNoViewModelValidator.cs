using FluentValidation;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using System.Collections.Generic;

namespace LXP.Common.Validators
{
    public class QuizQuestionNoViewModelValidator : AbstractValidator<QuizQuestionNoViewModel>
    {
        public QuizQuestionNoViewModelValidator()
        {
            RuleFor(question => question.Question)
                .NotEmpty().WithMessage("Question text is required.");

            RuleFor(question => question.QuestionType)
                .NotEmpty().WithMessage("Question type is required.")
                .Must(BeAValidQuestionType).WithMessage("Invalid question type.");

            RuleFor(question => question.Options)
                .NotEmpty().WithMessage("Options are required.")
                .Must((model, options) => ValidateOptionsByQuestionType(model.QuestionType, options))
                .WithMessage("Invalid options for the given question type.");
        }

        private bool BeAValidQuestionType(string questionType)
        {
            var validQuestionTypes = new List<string> { "MSQ", "MCQ", "T/F" };
            return validQuestionTypes.Contains(questionType.ToUpper());
        }

        private bool ValidateOptionsByQuestionType(string questionType, List<QuestionOptionViewModel> options)
        {
            switch (questionType.ToUpper())
            {
                case "MSQ":
                    return options.Count >= 5 && options.Count <= 8 && options.Count(o => o.IsCorrect) >= 2 && options.Count(o => o.IsCorrect) <= 3;
                case "MCQ":
                    return options.Count == 4 && options.Count(o => o.IsCorrect) == 1;
                case "T/F":
                    return options.Count == 2 && options.Count(o => o.IsCorrect) == 1 && options.Any(o => o.Option.ToLower() == "true") && options.Any(o => o.Option.ToLower() == "false");
                default:
                    return false;
            }
        }
    }
}