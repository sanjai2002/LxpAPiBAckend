using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class AllLearnersViewModel
    {
        public Guid LearnerID { get; set; }
        public string? LearnerName { get; set; }

        public string? Email { get; set; }

        public DateTime? LastLogin { get; set; }


    }
}
