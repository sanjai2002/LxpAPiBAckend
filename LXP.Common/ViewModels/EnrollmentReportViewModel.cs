using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class EnrollmentReportViewModel
    {
        ///<summary>
        ///Course Name
        ///</summary>

        public Guid CourseId { get; set; }
        ///<summary>
        ///Course Name
        /// </summary>
        public string CourseName { get; set; }

        ///<summary>
        ///Enrolled Users
        /// </summary>

        public int EnrolledUsers { get; set; }

        ///<summary>
        ///Inprogress Users
        /// </summary>

        public int InprogressUsers { get; set; }

        ///<summary>
        ///Completed Users
        /// </summary>

        public int CompletedUsers { get; set; }
    }
}
