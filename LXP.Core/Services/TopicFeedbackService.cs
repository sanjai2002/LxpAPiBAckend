using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

namespace LXP.Core.Services
{
    public class TopicFeedbackService : ITopicFeedbackService
    {
        private readonly ITopicFeedbackRepository _topicFeedbackRepository;

        public TopicFeedbackService(ITopicFeedbackRepository topicFeedbackRepository)
        {
            _topicFeedbackRepository = topicFeedbackRepository;
        }

        public Guid AddFeedbackQuestion(
            TopicFeedbackQuestionViewModel topicFeedbackQuestion,
            List<TopicFeedbackQuestionsOptionViewModel> options
        )
        {
            var normalizedQuestionType = topicFeedbackQuestion.QuestionType.ToUpper();

            if (normalizedQuestionType == FeedbackQuestionTypes.DescriptiveQuestion.ToUpper())
            {
                options = null;
            }

            if (!ValidateOptionsByFeedbackQuestionType(topicFeedbackQuestion.QuestionType, options))
                throw new ArgumentException(
                    "Invalid options for the given question type.",
                    nameof(options)
                );

            var questionEntity = new Topicfeedbackquestion
            {
                TopicId = topicFeedbackQuestion.TopicId,
                QuestionNo = _topicFeedbackRepository.GetNextFeedbackQuestionNo(
                    topicFeedbackQuestion.TopicId
                ),
                Question = topicFeedbackQuestion.Question,
                QuestionType = normalizedQuestionType,
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now
            };

            _topicFeedbackRepository.AddFeedbackQuestion(questionEntity);

            if (normalizedQuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                if (options != null && options.Count > 0)
                {
                    var optionEntities = options
                        .Select(option => new Feedbackquestionsoption
                        {
                            TopicFeedbackQuestionId = questionEntity.TopicFeedbackQuestionId,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin"
                        })
                        .ToList();

                    _topicFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
                }
            }

            return questionEntity.TopicFeedbackQuestionId;
        }

        public List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
        {
            return _topicFeedbackRepository.GetAllFeedbackQuestions();
        }

        public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(
            Guid topicFeedbackQuestionId
        )
        {
            return _topicFeedbackRepository.GetFeedbackQuestionById(topicFeedbackQuestionId);
        }

        public bool UpdateFeedbackQuestion(
            Guid topicFeedbackQuestionId,
            TopicFeedbackQuestionViewModel topicFeedbackQuestion,
            List<TopicFeedbackQuestionsOptionViewModel> options
        )
        {
            var existingQuestion = _topicFeedbackRepository.GetTopicFeedbackQuestionEntityById(
                topicFeedbackQuestionId
            );
            if (existingQuestion != null)
            {
                if (
                    !existingQuestion.QuestionType.Equals(
                        topicFeedbackQuestion.QuestionType,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    throw new InvalidOperationException("Question type cannot be modified.");
                }

                existingQuestion.Question = topicFeedbackQuestion.Question;
                existingQuestion.ModifiedAt = DateTime.Now;
                existingQuestion.ModifiedBy = "Admin";
                _topicFeedbackRepository.UpdateFeedbackQuestion(existingQuestion);

                if (
                    existingQuestion.QuestionType
                    == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper()
                )
                {
                    if (
                        !ValidateOptionsByFeedbackQuestionType(
                            existingQuestion.QuestionType,
                            options
                        )
                    )
                    {
                        throw new ArgumentException("Invalid options for the given question type.");
                    }

                    var existingOptions = _topicFeedbackRepository.GetFeedbackQuestionOptionsById(
                        topicFeedbackQuestionId
                    );
                    _topicFeedbackRepository.RemoveFeedbackQuestionOptions(existingOptions);

                    if (options != null && options.Count > 0)
                    {
                        var optionEntities = options
                            .Select(option => new Feedbackquestionsoption
                            {
                                TopicFeedbackQuestionId = topicFeedbackQuestionId,
                                OptionText = option.OptionText,
                                CreatedAt = DateTime.Now,
                                CreatedBy = "Admin"
                            })
                            .ToList();

                        _topicFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
                    }
                }

                return true;
            }
            return false;
        }

        public bool DeleteFeedbackQuestion(Guid topicFeedbackQuestionId)
        {
            try
            {
                var existingQuestion = _topicFeedbackRepository.GetTopicFeedbackQuestionEntityById(
                    topicFeedbackQuestionId
                );
                if (existingQuestion != null)
                {
                    var relatedOptions = _topicFeedbackRepository.GetFeedbackQuestionOptionsById(
                        topicFeedbackQuestionId
                    );

                    if (relatedOptions.Any())
                    {
                        _topicFeedbackRepository.RemoveFeedbackQuestionOptions(relatedOptions);
                    }

                    // Remove the FeedbackQuestion and related FeedbackResponses
                    _topicFeedbackRepository.RemoveFeedbackQuestion(existingQuestion);
                    _topicFeedbackRepository.ReorderQuestionNos(
                        existingQuestion.TopicId,
                        existingQuestion.QuestionNo
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while deleting the feedback question.",
                    ex
                );
            }
            return false;
        }

        public List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId)
        {
            return _topicFeedbackRepository.GetFeedbackQuestionsByTopicId(topicId);
        }

        public bool DeleteFeedbackQuestionsByTopicId(Guid topicId)
        {
            try
            {
                var questions = _topicFeedbackRepository.GetFeedbackQuestionsByTopicId(topicId);
                if (questions == null || questions.Count == 0)
                    return false;

                foreach (var question in questions)
                {
                    DeleteFeedbackQuestion(question.TopicFeedbackQuestionId);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ValidateOptionsByFeedbackQuestionType(
            string questionType,
            List<TopicFeedbackQuestionsOptionViewModel> options
        )
        {
            questionType = questionType.ToUpper();

            if (questionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                return options != null && options.Count >= 2 && options.Count <= 5;
            }
            return options == null || options.Count == 0;
        }
    }
}
