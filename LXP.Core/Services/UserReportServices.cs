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
    public class UserReportServices : IUserReportServices
    {
        private readonly IUserReportRepository _userReportRepository;
        public UserReportServices(IUserReportRepository userReportRepository)
        {
            _userReportRepository = userReportRepository;
        }

        public IEnumerable<UserReportViewModel> GetUserReport()
        {
           return _userReportRepository.GetUserReport();
        }
    }
}
