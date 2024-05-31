using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class GetLearnerViewModel
    {
        public string Email { get; set; } = null!;

        public string? Password { get; set; }

        public string Role { get; set; } = null!;
    }
}
