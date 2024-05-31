using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class LoginRole
    {

        public bool Email { get; set; }

        public bool Password { get; set; }

        public string? Role { get; set; }

        public bool AccountStatus { get; set; }

        public Guid GetLearnerId {  get; set; }


    }
}