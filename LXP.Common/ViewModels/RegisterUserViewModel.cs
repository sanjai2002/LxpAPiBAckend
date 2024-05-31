using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class RegisterUserViewModel
    {

        public string email { get; set; } = null!;

        public string? password { get; set; }

        public string role { get; set; } = null!;

        //public string CreatedBy { get; set; } = null!;

        public string firstName { get; set; } = null!;

        public string lastName { get; set; } = null!;

        public string? dob { get; set; }

        public string gender { get; set; } = null!;

        public string contactNumber { get; set; } = null!;

        public string stream { get; set; } = null!;

        public string? Newpassword { get; set; }

        //public string? ProfilePhoto { get; set; }

    }
}
