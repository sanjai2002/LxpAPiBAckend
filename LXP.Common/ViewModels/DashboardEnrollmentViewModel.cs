
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class DashboardEnrollmentViewModel
    {
        public Guid EnrollmentId { get; set; }
        public Guid LearnerId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

    }
}
