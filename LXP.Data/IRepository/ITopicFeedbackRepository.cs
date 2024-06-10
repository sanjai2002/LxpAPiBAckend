
using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;

namespace LXP.Data.IRepository
{
    public interface ITopicFeedbackRepository
    {
        void AddFeedbackQuestion(Topicfeedbackquestion questionEntity);
        void AddFeedbackQuestionOptions(List<Feedbackquestionsoption> options);
        List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
        List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId);
        int GetNextFeedbackQuestionNo(Guid topicId);
        TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId);
        void UpdateFeedbackQuestion(Topicfeedbackquestion questionEntity);
        void RemoveFeedbackQuestion(Topicfeedbackquestion questionEntity);
        void RemoveFeedbackQuestionOptions(List<Feedbackquestionsoption> options);
        void ReorderQuestionNos(Guid topicId, int deletedQuestionNo);
        List<Feedbackquestionsoption> GetFeedbackQuestionOptionsById(Guid topicFeedbackQuestionId);
        Topicfeedbackquestion GetTopicFeedbackQuestionEntityById(Guid topicFeedbackQuestionId);
    }
}