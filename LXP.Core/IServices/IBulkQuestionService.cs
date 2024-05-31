
using Microsoft.AspNetCore.Http;

namespace LXP.Core.IServices
{
    public interface IBulkQuestionService
    {
        Task<object> ImportQuizDataAsync(IFormFile file, Guid quizId);
    }
}
