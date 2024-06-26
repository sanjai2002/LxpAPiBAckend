using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class QuizFeedbackService : IQuizFeedbackService
    {
        private readonly IQuizFeedbackRepository _quizFeedbackRepository;

        public QuizFeedbackService(IQuizFeedbackRepository quizFeedbackRepository)
        {
            _quizFeedbackRepository = quizFeedbackRepository;
        }

        public Guid AddFeedbackQuestion(
            QuizfeedbackquestionViewModel quizfeedbackquestion,
            List<QuizFeedbackQuestionsOptionViewModel> options
        )
        {
            // Normalize question type to uppercase
            var normalizedQuestionType = quizfeedbackquestion.QuestionType.ToUpper();

            // Ensure no options are saved for descriptive questions
            if (normalizedQuestionType == FeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
            {
                options = null;
            }

            if (!ValidateOptionsByFeedbackQuestionType(quizfeedbackquestion.QuestionType, options))
            {
                throw new ArgumentException(
                    "Invalid options for the given question type.",
                    nameof(options)
                );
            }

            // Create the feedback question entity
            var questionEntity = new Quizfeedbackquestion
            {
                QuizId = quizfeedbackquestion.QuizId,
                QuestionNo = _quizFeedbackRepository.GetNextFeedbackQuestionNo(
                    quizfeedbackquestion.QuizId
                ),
                Question = quizfeedbackquestion.Question,
                QuestionType = normalizedQuestionType,
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now
            };

            _quizFeedbackRepository.AddFeedbackQuestion(questionEntity);

            // Save the options only if the question type is MCQ
            if (
                normalizedQuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper()
                && options != null
                && options.Count > 0
            )
            {
                var optionEntities = options
                    .Select(option => new Feedbackquestionsoption
                    {
                        QuizFeedbackQuestionId = questionEntity.QuizFeedbackQuestionId,
                        OptionText = option.OptionText,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Admin"
                    })
                    .ToList();

                _quizFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
            }

            return questionEntity.QuizFeedbackQuestionId;
        }

        public List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _quizFeedbackRepository.GetAllFeedbackQuestions();
        }

        public QuizfeedbackquestionNoViewModel GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
        {
            var feedbackQuestion = _quizFeedbackRepository.GetFeedbackQuestionEntityById(
                quizFeedbackQuestionId
            );

            if (feedbackQuestion == null)
            {
                return null;
            }

            var options = _quizFeedbackRepository.GetFeedbackQuestionOptions(
                feedbackQuestion.QuizFeedbackQuestionId
            );

            return new QuizfeedbackquestionNoViewModel
            {
                QuizFeedbackQuestionId = feedbackQuestion.QuizFeedbackQuestionId,
                QuizId = feedbackQuestion.QuizId,
                QuestionNo = feedbackQuestion.QuestionNo,
                Question = feedbackQuestion.Question,
                QuestionType = feedbackQuestion.QuestionType,
                Options = options
                    .Select(o => new QuizFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            };
        }

        public bool UpdateFeedbackQuestion(
            Guid quizFeedbackQuestionId,
            QuizfeedbackquestionViewModel quizfeedbackquestion,
            List<QuizFeedbackQuestionsOptionViewModel> options
        )
        {
            var existingQuestion = _quizFeedbackRepository.GetFeedbackQuestionEntityById(
                quizFeedbackQuestionId
            );

            if (existingQuestion == null)
            {
                return false;
            }

            // Check if the question type is being modified
            if (
                !existingQuestion.QuestionType.Equals(
                    quizfeedbackquestion.QuestionType,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                throw new InvalidOperationException("Question type cannot be modified.");
            }

            // Update the question details
            existingQuestion.Question = quizfeedbackquestion.Question;
            existingQuestion.ModifiedAt = DateTime.Now;
            existingQuestion.ModifiedBy = "Admin";
            _quizFeedbackRepository.UpdateFeedbackQuestion(existingQuestion);

            // Handle options only if the question type is MCQ
            if (
                existingQuestion.QuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper()
            )
            {
                if (!ValidateOptionsByFeedbackQuestionType(existingQuestion.QuestionType, options))
                {
                    throw new ArgumentException("Invalid options for the given question type.");
                }

                var existingOptions = _quizFeedbackRepository.GetFeedbackQuestionOptions(
                    quizFeedbackQuestionId
                );
                _quizFeedbackRepository.DeleteFeedbackQuestionOptions(existingOptions);

                if (options != null && options.Count > 0)
                {
                    var optionEntities = options
                        .Select(option => new Feedbackquestionsoption
                        {
                            QuizFeedbackQuestionId = quizFeedbackQuestionId,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin"
                        })
                        .ToList();

                    _quizFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
                }
            }

            return true;
        }

        public bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
        {
            var existingQuestion = _quizFeedbackRepository.GetFeedbackQuestionEntityById(
                quizFeedbackQuestionId
            );

            if (existingQuestion == null)
            {
                return false;
            }

            var relatedResponses = _quizFeedbackRepository.GetFeedbackResponsesByQuestionId(
                quizFeedbackQuestionId
            );
            if (relatedResponses.Any())
            {
                _quizFeedbackRepository.DeleteFeedbackResponses(relatedResponses);
            }

            var relatedOptions = _quizFeedbackRepository.GetFeedbackQuestionOptions(
                quizFeedbackQuestionId
            );
            if (relatedOptions.Any())
            {
                _quizFeedbackRepository.DeleteFeedbackQuestionOptions(relatedOptions);
            }

            _quizFeedbackRepository.DeleteFeedbackQuestion(existingQuestion);

            ReorderQuestionNos(existingQuestion.QuizId, existingQuestion.QuestionNo);

            return true;
        }

        public List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId)
        {
            return _quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);
        }

        public bool DeleteFeedbackQuestionsByQuizId(Guid quizId)
        {
            var questions = _quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);
            if (questions == null || questions.Count == 0)
            {
                return false;
            }

            foreach (var question in questions)
            {
                DeleteFeedbackQuestion(question.QuizFeedbackQuestionId);
            }

            return true;
        }

        private bool ValidateOptionsByFeedbackQuestionType(
            string questionType,
            List<QuizFeedbackQuestionsOptionViewModel> options
        )
        {
            questionType = questionType.ToUpper();

            if (questionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                return options != null && options.Count >= 2 && options.Count <= 5;
            }
            return options == null || options.Count == 0;
        }

        private void ReorderQuestionNos(Guid quizId, int deletedQuestionNo)
        {
            var questionsToUpdate = _quizFeedbackRepository
                .GetFeedbackQuestionsByQuizId(quizId)
                .Where(q => q.QuestionNo > deletedQuestionNo)
                .OrderBy(q => q.QuestionNo)
                .ToList();

            foreach (var question in questionsToUpdate)
            {
                var questionEntity = _quizFeedbackRepository.GetFeedbackQuestionEntityById(
                    question.QuizFeedbackQuestionId
                );
                if (questionEntity != null)
                {
                    questionEntity.QuestionNo--;
                    _quizFeedbackRepository.UpdateFeedbackQuestion(questionEntity);
                }
            }
        }
    }
}
