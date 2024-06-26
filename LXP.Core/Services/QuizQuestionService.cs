using LXP.Common.Constants;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class QuizQuestionService : IQuizQuestionService
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;

        public QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<Guid> AddQuestionAsync(
            QuizQuestionViewModel quizQuestion,
            List<QuestionOptionViewModel> options
        )
        {
            if (string.IsNullOrWhiteSpace(quizQuestion.Question))
                throw new ArgumentException(
                    "Question cannot be null or empty.",
                    nameof(quizQuestion.Question)
                );

            if (string.IsNullOrWhiteSpace(quizQuestion.QuestionType))
                throw new ArgumentException(
                    "QuestionType cannot be null or empty.",
                    nameof(quizQuestion.QuestionType)
                );

            quizQuestion.QuestionType = quizQuestion.QuestionType.ToUpper();

            if (!IsValidQuestionType(quizQuestion.QuestionType))
                throw new ArgumentException(
                    "Invalid question type.",
                    nameof(quizQuestion.QuestionType)
                );

            if (!ValidateOptions(quizQuestion.QuestionType, options))
                throw new ArgumentException(
                    "Invalid options for the given question type.",
                    nameof(options)
                );

            return await _quizQuestionRepository.AddQuestionAsync(quizQuestion, options);
        }

        public async Task<bool> UpdateQuestionAsync(
            Guid quizQuestionId,
            QuizQuestionViewModel quizQuestion,
            List<QuestionOptionViewModel> options
        )
        {
            var existingQuestion = await _quizQuestionRepository.GetQuestionByIdAsync(
                quizQuestionId
            );
            if (existingQuestion == null)
                throw new ArgumentException("Question not found.", nameof(quizQuestionId));

            if (quizQuestion.QuestionType.ToUpper() != existingQuestion.QuestionType)
                throw new InvalidOperationException("Question type cannot be updated.");

            if (!ValidateOptions(existingQuestion.QuestionType, options))
                throw new ArgumentException(
                    "Invalid options for the given question type.",
                    nameof(options)
                );

            return await _quizQuestionRepository.UpdateQuestionAsync(
                quizQuestionId,
                quizQuestion,
                options
            );
        }

        public async Task<bool> DeleteQuestionAsync(Guid quizQuestionId)
        {
            var existingQuestion = await _quizQuestionRepository.GetQuestionByIdAsync(
                quizQuestionId
            );
            if (existingQuestion == null)
                return false;

            bool deleted = await _quizQuestionRepository.DeleteQuestionAsync(quizQuestionId);
            if (deleted)
            {
                _quizQuestionRepository.ReorderQuestionNos(
                    existingQuestion.QuizId,
                    existingQuestion.QuestionNo
                );
            }
            return deleted;
        }

        public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync()
        {
            return await _quizQuestionRepository.GetAllQuestionsAsync();
        }

        public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId)
        {
            return await _quizQuestionRepository.GetAllQuestionsByQuizIdAsync(quizId);
        }

        public async Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId)
        {
            return await _quizQuestionRepository.GetQuestionByIdAsync(quizQuestionId);
        }

        private bool IsValidQuestionType(string questionType)
        {
            return questionType == QuizQuestionTypes.MultiSelectQuestion
                || questionType == QuizQuestionTypes.MultiChoiceQuestion
                || questionType == QuizQuestionTypes.TrueFalseQuestion;
        }

        private bool ValidateOptions(string questionType, List<QuestionOptionViewModel> options)
        {
            if (questionType == QuizQuestionTypes.TrueFalseQuestion)
            {
                return ValidateTrueFalseOptions(options);
            }
            else
            {
                return ValidateUniqueOptions(options)
                    && ValidateOptionsByQuestionType(questionType, options);
            }
        }

        private bool ValidateTrueFalseOptions(List<QuestionOptionViewModel> options)
        {
            if (options.Count != 2)
                return false;

            var trueOption = options.Any(o => o.Option.ToLower() == "true");
            var falseOption = options.Any(o => o.Option.ToLower() == "false");

            return trueOption && falseOption;
        }

        private bool ValidateUniqueOptions(List<QuestionOptionViewModel> options)
        {
            var distinctOptions = options.Select(o => o.Option.ToLower()).Distinct().Count();
            return distinctOptions == options.Count;
        }

        private bool ValidateOptionsByQuestionType(
            string questionType,
            List<QuestionOptionViewModel> options
        )
        {
            switch (questionType)
            {
                case QuizQuestionTypes.MultiSelectQuestion:
                    return options.Count >= 5
                        && options.Count <= 8
                        && options.Count(o => o.IsCorrect) >= 2
                        && options.Count(o => o.IsCorrect) <= 3;
                case QuizQuestionTypes.MultiChoiceQuestion:
                    return options.Count == 4 && options.Count(o => o.IsCorrect) == 1;
                case QuizQuestionTypes.TrueFalseQuestion:
                    return options.Count == 2 && options.Count(o => o.IsCorrect) == 1;
                default:
                    return false;
            }
        }
    }
}
