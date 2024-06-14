using LXP.Common.ViewModels.QuizQuestionViewModel;

namespace LXP.Data.IRepository
{
    public interface IQuizQuestionRepository
    {
        Task<Guid> AddQuestionAsync(
            QuizQuestionViewModel quizQuestion,
            List<QuestionOptionViewModel> options
        );
        Task<bool> UpdateQuestionAsync(
            Guid quizQuestionId,
            QuizQuestionViewModel quizQuestion,
            List<QuestionOptionViewModel> options
        );
        Task<bool> DeleteQuestionAsync(Guid quizQuestionId);
        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync();
        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId);
        Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId);
        Task<int> GetNextQuestionNoAsync(Guid quizId);
    }
}

//using LXP.Common.ViewModels.QuizQuestionViewModel;

//namespace LXP.Data.IRepository
//{
//    public interface IQuizQuestionRepository
//    {
//        void DecrementQuestionNos(Guid deletedQuestionId);
//        int GetNextQuestionNo(Guid quizId);
//        Guid AddQuestionOption(QuestionOptionViewModel questionOption, Guid quizQuestionId);
//        List<QuestionOptionViewModel> GetQuestionOptionsById(Guid quizQuestionId);

//        bool ValidateOptionsByQuestionType(
//            string questionType,
//            List<QuestionOptionViewModel> options
//        );
//        Task<int> GetNextQuestionNoAsync(Guid quizId);

//        Task<Guid> AddQuestionAsync(
//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        );
//        Task<bool> UpdateQuestionAsync(
//            Guid quizQuestionId,
//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        );
//        Task<bool> DeleteQuestionAsync(Guid quizQuestionId);
//        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync();
//        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId);
//        Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId);
//    }
//}
