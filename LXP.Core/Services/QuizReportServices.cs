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

        public IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid)
        {
            //double Passmark = _quizReportRepository.FindPassmark(Quizid);

            return _quizReportRepository.GetPassdLearnersList(Quizid);

        }

        public IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid)
        {
            return _quizReportRepository.GetFailedLearnersList(Quizid);
        }
    }
}
