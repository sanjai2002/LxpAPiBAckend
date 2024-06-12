using LXP.Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class MaterialUpdateViewModel
    {

        public string MaterialId { get; set; }

        public string Name { get; set; } = null!;

        public IFormFile Material { get; set; } = null!;

        public string? ModifiedBy { get; set; }

       

       
    }

}

