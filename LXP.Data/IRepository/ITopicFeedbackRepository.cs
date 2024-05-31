


using LXP.Common.ViewModels.TopicFeedbackQuestionViemModel;


namespace LXP.Data.IRepository
{
    public interface ITopicFeedbackRepository
    {
        IEnumerable<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
        TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid id);

        bool AddFeedbackQuestion(TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options);
        bool DeleteFeedbackQuestion(Guid id);
        bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionViewModel question, List<FeedbackOptionDTO> options);
        IEnumerable<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId);

    }
}

//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LXP.Data.IRepository
//{
//    public interface ITopicFeedbackRepository
//    {
//        IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions();
//        TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id);
//        void AddFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse);
//       // bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options);
//        bool DeleteFeedbackQuestion(Guid id);
//        bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options);
//        IEnumerable<TopicFeedbackQuestionNoDTO> GetFeedbackQuestionsByTopicId(Guid topicId);

//        bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options, Guid topicId);
//    }
//}