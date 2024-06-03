using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class HighestEnrolledCourseViewModel
    {
        public Guid Courseid { get; set; }

        public string? CourseName { get; set; }

        public string? Thumbnailimage {  get; set; }

        public int Learnerscount {  get; set; }


    }
}
