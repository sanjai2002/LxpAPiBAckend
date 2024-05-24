using LXP.Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class MaterialViewModel
    {

        public string TopicId { get; set; }

        public string MaterialTypeId { get; set; }

        public string Name { get; set; }

        public IFormFile Material { get; set; }

        public decimal Duration { get; set; }

        
        public string CreatedBy { get; set; } 

      
    }
}
