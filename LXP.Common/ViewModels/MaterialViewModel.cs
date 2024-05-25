using Microsoft.AspNetCore.Http;

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
