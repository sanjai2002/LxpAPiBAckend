using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface IQuizReportRepository
    {
        IEnumerable<QuizReportViewModel> GetQuizReports();

    }
}
