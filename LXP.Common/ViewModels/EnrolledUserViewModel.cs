using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class EnrolledUserViewModel
    {
        public Guid LearnerId { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public sbyte Status { get; set; }

        ///<summary>
        ///Email ID
        /// </summary>
        public string? EmailId { get; set; }
    }
}
