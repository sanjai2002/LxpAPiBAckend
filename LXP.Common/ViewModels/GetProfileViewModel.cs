using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class GetProfileViewModel
    {

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Dob { get; set; }

        public string Gender { get; set; } = null!;

        public string ContactNumber { get; set; } = null!;

        public string Stream { get; set; } = null!;

        public string? ProfilePhoto { get; set; }
    }
}
