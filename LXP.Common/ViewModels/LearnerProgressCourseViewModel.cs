using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class LearnerProgressCourseViewModel
    {
        public Guid CourseId { get; set; }
        public Guid LevelId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TimeOnly Duration { get; set; }
        public string Thumbnail { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public ICollection<LearnerProgressTopicViewModel> Topics { get; set; } = new List<LearnerProgressTopicViewModel>();
    }
}
