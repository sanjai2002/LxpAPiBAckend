using LXP.Common.Entities;
using System;

namespace LXP.Data.IRepository
{
    public interface IFeedbackResponseRepository
    {
        Quizfeedbackquestion GetQuizFeedbackQuestion(Guid quizFeedbackQuestionId);
        Topicfeedbackquestion GetTopicFeedbackQuestion(Guid topicFeedbackQuestionId);
        Learner GetLearner(Guid learnerId);
        Feedbackresponse GetExistingQuizFeedbackResponse(Guid quizFeedbackQuestionId, Guid learnerId);
        Feedbackresponse GetExistingTopicFeedbackResponse(Guid topicFeedbackQuestionId, Guid learnerId);
        void AddFeedbackResponse(Feedbackresponse feedbackResponse);
        Guid? GetOptionIdByText(Guid questionId, string optionText);
        //LearnerProfile GetLearnerProfile(Guid learnerId);
    }
}
















//using LXP.Common.ViewModels.FeedbackResponseViewModel;

//namespace LXP.Data.IRepository
//{
//    public interface IFeedbackResponseRepository
//    {
//        void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse);
//        void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse);
//    }
//}