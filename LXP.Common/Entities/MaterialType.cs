using System;
using System.Collections.Generic;

namespace LXP.Common.Entities;

public partial class MaterialType
{
    public Guid MaterialTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
