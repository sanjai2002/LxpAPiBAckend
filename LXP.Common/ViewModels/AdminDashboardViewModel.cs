using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public  class AdminDashboardViewModel
    {
        public int NoOfLearners {  get; set; }

        public int NoOfCourse {  get; set; }

        public int NoOfActiveLearners { get; set; }

        public List<string> HighestEnrolledCourse {  get; set; }

       
    }
}
