using LXP.Common.Entities;
using FluentValidation;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using LXP.Services.IServices;
using LXP.Common.Validators;
using LXP.Common.Constants;


namespace LXP.Services
{
    public class FeedbackResponseService : IFeedbackResponseService
    {
        private readonly IFeedbackResponseRepository _feedbackResponseRepository;
        private readonly QuizFeedbackResponseViewModelValidator _quizFeedbackValidator;
        private readonly TopicFeedbackResponseViewModelValidator _topicFeedbackValidator;

        public FeedbackResponseService(IFeedbackResponseRepository feedbackResponseRepository)
        {
            _feedbackResponseRepository = feedbackResponseRepository;
            _quizFeedbackValidator = new QuizFeedbackResponseViewModelValidator();
            _topicFeedbackValidator = new TopicFeedbackResponseViewModelValidator();
        }

        public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse)
        {
            ValidateAndSubmitQuizFeedback(feedbackResponse);
        }

        public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
        {
            ValidateAndSubmitTopicFeedback(feedbackResponse);
        }

        public void SubmitFeedbackResponses(IEnumerable<QuizFeedbackResponseViewModel> feedbackResponses)
        {
            foreach (var feedbackResponse in feedbackResponses)
            {
                ValidateAndSubmitQuizFeedback(feedbackResponse);
            }
        }

        public void SubmitFeedbackResponses(IEnumerable<TopicFeedbackResponseViewModel> feedbackResponses)
        {
            foreach (var feedbackResponse in feedbackResponses)
            {
                ValidateAndSubmitTopicFeedback(feedbackResponse);
            }
        }

        private void ValidateAndSubmitQuizFeedback(QuizFeedbackResponseViewModel feedbackResponse)
        {
            var validationResult = _quizFeedbackValidator.Validate(feedbackResponse);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (feedbackResponse == null)
                throw new ArgumentNullException(nameof(feedbackResponse));

            var question = _feedbackResponseRepository.GetQuizFeedbackQuestion(feedbackResponse.QuizFeedbackQuestionId);

            if (question == null)
                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.QuizFeedbackQuestionId));

            var learner = _feedbackResponseRepository.GetLearner(feedbackResponse.LearnerId);

            if (learner == null)
                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

            var existingResponse = _feedbackResponseRepository.GetExistingQuizFeedbackResponse(feedbackResponse.QuizFeedbackQuestionId, feedbackResponse.LearnerId);

            if (existingResponse != null)
                throw new InvalidOperationException("User has already submitted a response for this question.");

            Guid? optionId = null;

            if (question.QuestionType == QuizFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
                    throw new ArgumentException("Option text must be provided for MCQ responses.");

                optionId = _feedbackResponseRepository.GetOptionIdByText(feedbackResponse.QuizFeedbackQuestionId, feedbackResponse.OptionText);
                if (optionId == null)
                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

                feedbackResponse.Response = null;
            }

            //var learnerProfile = _feedbackResponseRepository.GetLearnerProfile(feedbackResponse.LearnerId);
            //var generatedBy = $"{learnerProfile.FirstName} {learnerProfile.LastName}";

            var response = new Feedbackresponse
            {
                QuizFeedbackQuestionId = feedbackResponse.QuizFeedbackQuestionId,
                LearnerId = feedbackResponse.LearnerId,
                Response = feedbackResponse.Response,
                OptionId = optionId,
                GeneratedAt = DateTime.UtcNow,
                GeneratedBy = "learner"
            };

            _feedbackResponseRepository.AddFeedbackResponse(response);
            feedbackResponse.QuizId = question.QuizId;
        }

        private void ValidateAndSubmitTopicFeedback(TopicFeedbackResponseViewModel feedbackResponse)
        {
            var validationResult = _topicFeedbackValidator.Validate(feedbackResponse);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (feedbackResponse == null)
                throw new ArgumentNullException(nameof(feedbackResponse));

            var question = _feedbackResponseRepository.GetTopicFeedbackQuestion(feedbackResponse.TopicFeedbackQuestionId);

            if (question == null)
                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.TopicFeedbackQuestionId));

            var learner = _feedbackResponseRepository.GetLearner(feedbackResponse.LearnerId);

            if (learner == null)
                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

            var existingResponse = _feedbackResponseRepository.GetExistingTopicFeedbackResponse(feedbackResponse.TopicFeedbackQuestionId, feedbackResponse.LearnerId);

            if (existingResponse != null)
                throw new InvalidOperationException("User has already submitted a response for this question.");

            Guid? optionId = null;

            if (question.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
                    throw new ArgumentException("Option text must be provided for MCQ responses.");

                optionId = _feedbackResponseRepository.GetOptionIdByText(feedbackResponse.TopicFeedbackQuestionId, feedbackResponse.OptionText);
                if (optionId == null)
                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

                feedbackResponse.Response = null;
            }

            //var learnerProfile = _feedbackResponseRepository.GetLearnerProfile(feedbackResponse.LearnerId);
            //var generatedBy = $"{learnerProfile.FirstName} {learnerProfile.LastName}";

            var response = new Feedbackresponse
            {
                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
                LearnerId = feedbackResponse.LearnerId,
                Response = feedbackResponse.Response,
                OptionId = optionId,
                GeneratedAt = DateTime.UtcNow,
                GeneratedBy = "learner"
            };

            _feedbackResponseRepository.AddFeedbackResponse(response);
            feedbackResponse.TopicId = question.TopicId;
        }
    }
}






















//using LXP.Common.ViewModels.FeedbackResponseViewModel;
//using LXP.Data.IRepository;
//using LXP.Services.IServices;

//namespace LXP.Services
//{
//    public class FeedbackResponseService : IFeedbackResponseService
//    {
//        private readonly IFeedbackResponseRepository _feedbackResponseRepository;

//        public FeedbackResponseService(IFeedbackResponseRepository feedbackResponseRepository)
//        {
//            _feedbackResponseRepository = feedbackResponseRepository;
//        }

//        public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse)
//        {
//            _feedbackResponseRepository.SubmitFeedbackResponse(feedbackResponse);
//        }

//        public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
//        {
//            _feedbackResponseRepository.SubmitFeedbackResponse(feedbackResponse);
//        }
//    }
//}