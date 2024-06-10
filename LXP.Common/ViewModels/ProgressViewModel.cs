using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class ProgressViewModel
    {
        public Guid MaterialId { get; set; }

        public Guid LearnerId { get; set; }

        public TimeOnly WatchTime { get; set; }
    }
}
