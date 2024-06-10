using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class LearnerProgressEnrollmentViewModel
    {
        public Guid EnrollmentId { get; set; }
        public Guid LearnerId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool EnrollStatus { get; set; }
        public sbyte CompletedStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public decimal CourseCompletionPercentage { get; set; }

        public LearnerProgressCourseViewModel Course { get; set; } = null!;
    }
}
