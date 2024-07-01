using LXP.Common.ViewModels.QuizViewModel;

namespace LXP.Core.IServices
{
    public interface IQuizService
    {
        QuizViewModel GetQuizById(Guid quizId);
        IEnumerable<QuizViewModel> GetAllQuizzes();
        void CreateQuiz(QuizViewModel quiz, Guid topicId);
        void UpdateQuiz(QuizViewModel quiz);
        void DeleteQuiz(Guid quizId);
        Guid? GetQuizIdByTopicId(Guid topicId);
    }
}
