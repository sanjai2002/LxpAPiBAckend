using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class MaterialListViewModel
    {
        public Guid MaterialId { get; set; }

        public string TopicName { get; set; }

        public string MaterialType { get; set; }

        public string Name { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public decimal Duration { get; set; }

        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        public string? CreatedBy { get; set; } = null;

        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; } = null;

        public string? ModifiedAt { get; set; } = null!;
    }
}
