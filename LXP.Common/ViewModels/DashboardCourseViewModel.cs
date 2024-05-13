using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class DashboardCourseViewModel
    {
        public Guid CourseId { get; set; }
        public string ? Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
