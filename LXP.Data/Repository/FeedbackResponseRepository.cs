
using FluentValidation;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using LXP.Common.Validators;
using LXP.Common.Entities;

namespace LXP.Data.Repository
{
    public class FeedbackResponseRepository : IFeedbackResponseRepository
    {
        private readonly LXPDbContext _context;
        private readonly IValidator<QuizFeedbackResponseViewModel> _quizFeedbackValidator;
        private readonly IValidator<TopicFeedbackResponseViewModel> _topicFeedbackValidator;

        public FeedbackResponseRepository(LXPDbContext context)
        {
            _context = context;
            _quizFeedbackValidator = new QuizFeedbackResponseViewModelValidator();
            _topicFeedbackValidator = new TopicFeedbackResponseViewModelValidator();
        }

        public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse)
        {
            var validationResult = _quizFeedbackValidator.Validate(feedbackResponse);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (feedbackResponse == null)
                throw new ArgumentNullException(nameof(feedbackResponse));

            var question = _context.Quizfeedbackquestions
                .FirstOrDefault(q => q.QuizFeedbackQuestionId == feedbackResponse.QuizFeedbackQuestionId);

            if (question == null)
                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.QuizFeedbackQuestionId));

            var learner = _context.Learners
                .FirstOrDefault(l => l.LearnerId == feedbackResponse.LearnerId);

            if (learner == null)
                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

            var existingResponse = _context.Feedbackresponses
                .FirstOrDefault(r => r.QuizFeedbackQuestionId == feedbackResponse.QuizFeedbackQuestionId &&
                                     r.LearnerId == feedbackResponse.LearnerId);

            if (existingResponse != null)
                throw new InvalidOperationException("User has already submitted a response for this question.");

            Guid? optionId = null;

            if (question.QuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
                    throw new ArgumentException("Option text must be provided for MCQ responses.");

                optionId = GetOptionIdByText(feedbackResponse.QuizFeedbackQuestionId, feedbackResponse.OptionText);
                if (optionId == null)
                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

                feedbackResponse.Response = null;
            }

            var response = new Feedbackresponse
            {
                QuizFeedbackQuestionId = feedbackResponse.QuizFeedbackQuestionId,
                LearnerId = feedbackResponse.LearnerId,
                Response = feedbackResponse.Response,
                OptionId = optionId,
                GeneratedAt = DateTime.UtcNow,
                GeneratedBy = "User"
            };

            _context.Feedbackresponses.Add(response);
            _context.SaveChanges();
        }

        public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
        {
            var validationResult = _topicFeedbackValidator.Validate(feedbackResponse);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (feedbackResponse == null)
                throw new ArgumentNullException(nameof(feedbackResponse));

            var question = _context.Topicfeedbackquestions
                .FirstOrDefault(q => q.TopicFeedbackQuestionId == feedbackResponse.TopicFeedbackQuestionId);

            if (question == null)
                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.TopicFeedbackQuestionId));

            var learner = _context.Learners
                .FirstOrDefault(l => l.LearnerId == feedbackResponse.LearnerId);

            if (learner == null)
                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

            var existingResponse = _context.Feedbackresponses
                .FirstOrDefault(r => r.TopicFeedbackQuestionId == feedbackResponse.TopicFeedbackQuestionId &&
                                     r.LearnerId == feedbackResponse.LearnerId);

            if (existingResponse != null)
                throw new InvalidOperationException("User has already submitted a response for this question.");

            Guid? optionId = null;

            if (question.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
            {
                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
                    throw new ArgumentException("Option text must be provided for MCQ responses.");

                optionId = GetOptionIdByText(feedbackResponse.TopicFeedbackQuestionId, feedbackResponse.OptionText);
                if (optionId == null)
                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

                feedbackResponse.Response = null;
            }

            var response = new Feedbackresponse
            {
                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
                LearnerId = feedbackResponse.LearnerId,
                Response = feedbackResponse.Response,
                OptionId = optionId,
                GeneratedAt = DateTime.UtcNow,
                GeneratedBy = "User"
            };

            _context.Feedbackresponses.Add(response);
            _context.SaveChanges();
        }

        private Guid? GetOptionIdByText(Guid questionId, string optionText)
        {
            var option = _context.Feedbackquestionsoptions
                .FirstOrDefault(o => o.QuizFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower()) ??
                          _context.Feedbackquestionsoptions
                .FirstOrDefault(o => o.TopicFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower());

            return option?.FeedbackQuestionOptionId;
        }
    }
}

//using System;
//using System.Linq;
//using LXP.Common.ViewModels.FeedbackResponseViewModel;
//using LXP.Data.DBContexts;
//using LXP.Data.IRepository;

//namespace LXP.Data.Repository
//{
//    public class FeedbackResponseRepository : IFeedbackResponseRepository
//    {
//        private readonly LXPDbContext _context;

//        public FeedbackResponseRepository(LXPDbContext context)
//        {
//            _context = context;
//        }

//        public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse)
//        {
//            if (feedbackResponse == null)
//                throw new ArgumentNullException(nameof(feedbackResponse));

//            var question = _context.Quizfeedbackquestions
//                .FirstOrDefault(q => q.QuizFeedbackQuestionId == feedbackResponse.QuizFeedbackQuestionId);

//            if (question == null)
//                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.QuizFeedbackQuestionId));

//            var learner = _context.Learners
//                .FirstOrDefault(l => l.LearnerId == feedbackResponse.LearnerId);

//            if (learner == null)
//                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

//            var existingResponse = _context.Feedbackresponses
//                .FirstOrDefault(r => r.QuizFeedbackQuestionId == feedbackResponse.QuizFeedbackQuestionId &&
//                                     r.LearnerId == feedbackResponse.LearnerId);

//            if (existingResponse != null)
//                throw new InvalidOperationException("User has already submitted a response for this question.");

//            Guid? optionId = null;

//            if (question.QuestionType == FeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
//            {
//                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
//                    throw new ArgumentException("Option text must be provided for MCQ responses.");

//                optionId = GetOptionIdByText(feedbackResponse.QuizFeedbackQuestionId, feedbackResponse.OptionText);
//                if (optionId == null)
//                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

//                feedbackResponse.Response = null;
//            }

//            var response = new Feedbackresponse
//            {
//                QuizFeedbackQuestionId = feedbackResponse.QuizFeedbackQuestionId,
//                LearnerId = feedbackResponse.LearnerId,
//                Response = feedbackResponse.Response,
//                OptionId = optionId,
//                GeneratedAt = DateTime.UtcNow,
//                GeneratedBy = "User"
//            };

//            _context.Feedbackresponses.Add(response);
//            _context.SaveChanges();
//        }

//        public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse)
//        {
//            if (feedbackResponse == null)
//                throw new ArgumentNullException(nameof(feedbackResponse));

//            var question = _context.Topicfeedbackquestions
//                .FirstOrDefault(q => q.TopicFeedbackQuestionId == feedbackResponse.TopicFeedbackQuestionId);

//            if (question == null)
//                throw new ArgumentException("Invalid feedback question ID.", nameof(feedbackResponse.TopicFeedbackQuestionId));

//            var learner = _context.Learners
//                .FirstOrDefault(l => l.LearnerId == feedbackResponse.LearnerId);

//            if (learner == null)
//                throw new ArgumentException("Invalid learner ID.", nameof(feedbackResponse.LearnerId));

//            var existingResponse = _context.Feedbackresponses
//                .FirstOrDefault(r => r.TopicFeedbackQuestionId == feedbackResponse.TopicFeedbackQuestionId &&
//                                     r.LearnerId == feedbackResponse.LearnerId);

//            if (existingResponse != null)
//                throw new InvalidOperationException("User has already submitted a response for this question.");

//            Guid? optionId = null;

//            if (question.QuestionType == TopicFeedbackQuestionTypes.MultiChoiceQuestion.ToUpper())
//            {
//                if (string.IsNullOrEmpty(feedbackResponse.OptionText))
//                    throw new ArgumentException("Option text must be provided for MCQ responses.");

//                optionId = GetOptionIdByText(feedbackResponse.TopicFeedbackQuestionId, feedbackResponse.OptionText);
//                if (optionId == null)
//                    throw new ArgumentException("Invalid option text provided.", nameof(feedbackResponse.OptionText));

//                feedbackResponse.Response = null;
//            }

//            var response = new Feedbackresponse
//            {
//                TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
//                LearnerId = feedbackResponse.LearnerId,
//                Response = feedbackResponse.Response,
//                OptionId = optionId,
//                GeneratedAt = DateTime.UtcNow,
//                GeneratedBy = "User"
//            };

//            _context.Feedbackresponses.Add(response);
//            _context.SaveChanges();
//        }

//        private Guid? GetOptionIdByText(Guid questionId, string optionText)
//        {
//            var option = _context.Feedbackquestionsoptions
//                .FirstOrDefault(o => o.QuizFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower()) ??
//                          _context.Feedbackquestionsoptions
//                .FirstOrDefault(o => o.TopicFeedbackQuestionId == questionId && o.OptionText.ToLower() == optionText.ToLower());

//            return option?.FeedbackQuestionOptionId;
//        }
//    }
//}