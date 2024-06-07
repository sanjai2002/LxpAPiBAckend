using LXP.Common.Entities;
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

        ///<summary>
        ///Learner Id
        ///</summary>

        public Guid LearnerId { get; set; }

        ///<summary>
        ///LearnerName
        /// </summary>
        public string LearnerName { get; set; }

        ///<summary>
        ///Profile photo
        /// </summary>
        public string? ProfilePhoto { get; set; }


        ///<summary>
        ///Email ID
        /// </summary>
        public string? EmailId { get; set; }
    }

}