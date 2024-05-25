//using Microsoft.AspNetCore.Http;

//namespace LXP.Core.IServices
//{
//    public interface IBulkQuestionService
//    {
//        object ImportQuizData(IFormFile file, Guid quizId);
//    }
using Microsoft.AspNetCore.Http;

namespace LXP.Core.IServices
{
    public interface IBulkQuestionService
    {
        Task<object> ImportQuizDataAsync(IFormFile file, Guid quizId);
    }
}
