using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class LearnerProgressTopicViewModel
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<LearnerProgressMaterialViewModel> Materials { get; set; } = new List<LearnerProgressMaterialViewModel>();
    }
}
