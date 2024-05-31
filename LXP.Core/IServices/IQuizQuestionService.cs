
using LXP.Common.ViewModels.QuizQuestionViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface IQuizQuestionService
    {
        Task<Guid> AddQuestionAsync(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options);
        Task<bool> UpdateQuestionAsync(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options);
        Task<bool> DeleteQuestionAsync(Guid quizQuestionId);
        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId);
        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync();
        Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId);
    }
}


//using LXP.Common.ViewModels.QuizQuestionViewModel;


//namespace LXP.Core.IServices
//{
//    public interface IQuizQuestionService
//    {
//        Guid AddQuestion(QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options);
//        bool UpdateQuestion(Guid quizQuestionId, QuizQuestionViewModel quizQuestionDto, List<QuestionOptionViewModel> options);
//        bool DeleteQuestion(Guid quizQuestionId);
//        List<QuizQuestionNoViewModel> GetAllQuestionsByQuizId(Guid quizId);
//        List<QuizQuestionNoViewModel> GetAllQuestions();
//        QuizQuestionNoViewModel GetQuestionById(Guid quizQuestionId);
//    }
//}





//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;

//namespace LXP.Core.IServices
//{
//    public interface IQuizQuestionService
//    {
//        Guid AddQuestion(QuizQuestionDto quizQuestionDto, List<QuestionOptionDto> options);
//        bool UpdateQuestion(Guid quizQuestionId, QuizQuestionDto quizQuestionDto, List<QuestionOptionDto> options);
//        bool DeleteQuestion(Guid quizQuestionId);
//        List<QuizQuestionNoDto> GetAllQuestions();
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LXP.Core.IServices
//{
//    internal interface Interface1
//    {
//    }
//}
//public interface IQuizQuestionService
//{
//    IEnumerable<QuizQuestionDto> GetByQuizId(Guid quizId);
//    QuizQuestionDto GetById(Guid questionId);
//    void Create(QuizQuestionDto question);
//    void Update(QuizQuestionDto question);
//    void Delete(Guid questionId);
//}
//public interface IQuizQuestionService
//{
//    Task<IEnumerable<QuizQuestionDto>> GetByQuizId(Guid quizId);
//    Task<QuizQuestionDto> GetById(Guid questionId);
//    Task<QuizQuestionDto> AddQuestionAsync(QuizQuestionDto question);
//    Task UpdateQuestionAsync(QuizQuestionDto question);
//    Task DeleteQuestionAsync(Guid questionId);
//}