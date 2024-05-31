using FluentValidation;
using LXP.Common.ViewModels;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using System.Linq;

namespace LXP.Common.Validators
{
    public class BulkQuizQuestionViewModelValidator : AbstractValidator<BulkQuizQuestionViewModel>
    {
        public BulkQuizQuestionViewModelValidator()
        {
            RuleFor(q => q.QuestionType)
                .NotEmpty().WithMessage("Question type is required.")
                .Must(BeAValidQuestionType).WithMessage("Invalid question type.");

            RuleFor(q => q.Question)
                .NotEmpty().WithMessage("Question is required.");

            RuleFor(q => q)
                .Must(ValidateOptions).WithMessage("Invalid options provided for the question type.");

            RuleFor(q => q)
                .Must(ValidateCorrectOptions).WithMessage("Invalid correct options provided for the question type.");
        }

        private bool BeAValidQuestionType(string questionType)
        {
            return questionType == "MCQ" || questionType == "TF" || questionType == "MSQ";
        }

        private bool ValidateOptions(BulkQuizQuestionViewModel quizQuestion)
        {
            if (quizQuestion.QuestionType == "MCQ")
            {
                return quizQuestion.Options != null &&
                       quizQuestion.Options.Length == 4 &&
                       quizQuestion.Options.Distinct().Count() == 4;
            }
            else if (quizQuestion.QuestionType == "TF")
            {
                return quizQuestion.Options != null &&
                       quizQuestion.Options.Length == 2 &&
                       !quizQuestion.Options[0].Equals(quizQuestion.Options[1], System.StringComparison.OrdinalIgnoreCase);
            }
            else if (quizQuestion.QuestionType == "MSQ")
            {
                int optionCount = quizQuestion.Options?.Length ?? 0;
                return quizQuestion.Options != null &&
                       optionCount >= 4 && optionCount <= 11 &&
                       quizQuestion.Options.Distinct().Count() == optionCount;
            }
            return false;
        }

        private bool ValidateCorrectOptions(BulkQuizQuestionViewModel quizQuestion)
        {
            if (quizQuestion.QuestionType == "MCQ")
            {
                return quizQuestion.CorrectOptions != null &&
                       quizQuestion.CorrectOptions.Length == 1 &&
                       quizQuestion.Options.Contains(quizQuestion.CorrectOptions[0]);
            }
            else if (quizQuestion.QuestionType == "TF")
            {
                return quizQuestion.CorrectOptions != null &&
                       quizQuestion.CorrectOptions.Length == 1 &&
                       (quizQuestion.CorrectOptions[0].Equals("true", System.StringComparison.OrdinalIgnoreCase) ||
                        quizQuestion.CorrectOptions[0].Equals("false", System.StringComparison.OrdinalIgnoreCase));
            }
            else if (quizQuestion.QuestionType == "MSQ")
            {
                int correctOptionCount = quizQuestion.CorrectOptions?.Length ?? 0;
                return quizQuestion.CorrectOptions != null &&
                       correctOptionCount >= 2 && correctOptionCount <= 3 &&
                       quizQuestion.CorrectOptions.All(opt => quizQuestion.Options.Contains(opt));
            }
            return false;
        }
    }
}
