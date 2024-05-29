using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public class QuizReportServices : IQuizReportServices
    {
        private readonly IQuizReportRepository _quizReportRepository;
        public QuizReportServices(IQuizReportRepository quizReportRepository)
        {
            _quizReportRepository = quizReportRepository;
        }

        public IEnumerable<QuizReportViewModel> GetQuizReports()
        {
            return _quizReportRepository.GetQuizReports();
        }
    }
}
