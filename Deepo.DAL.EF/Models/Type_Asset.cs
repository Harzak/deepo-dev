using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Type_Asset
{
    public int Type_Asset_ID { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
