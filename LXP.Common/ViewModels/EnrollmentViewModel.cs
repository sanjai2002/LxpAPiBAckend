namespace LXP.Common.ViewModels
{
    public class EnrollmentViewModel
    {
        ///<summary>
        ///Course Name
        ///</summary>

        public Guid CourseId { get; set; }

        ///<summary>
        ///Learner Name
        ///</summary>

        public Guid LearnerId { get; set; }

        ///<summary>
        ///EnrollmentDate
        ///</summary>
        ///<example>02/02/2024</example>

        public DateTime EnrollmentDate { get; set; }

        ///<summary>
        ///Enrollment Status
        ///</summary>

        public bool EnrollStatus { get; set; }

        ///<summary>
        ///Enroll request Status
        ///</summary>

        public bool EnrollRequestStatus { get; set; }


    }
}
