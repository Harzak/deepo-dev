using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Asset
{
    public int Asset_ID { get; set; }

    public string Content_URL { get; set; } = null!;

    public string Content_Min_URL { get; set; } = null!;

    public int? Type_Asset_ID { get; set; }

    public virtual ICollection<Asset_Release> Asset_Releases { get; set; } = new List<Asset_Release>();

    public virtual Type_Asset? Type_Asset { get; set; }
}
