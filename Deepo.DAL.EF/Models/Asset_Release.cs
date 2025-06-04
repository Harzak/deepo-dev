using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Asset_Release
{
    public int Asset_Release_ID { get; set; }

    public int Release_ID { get; set; }

    public int Asset_ID { get; set; }

    public virtual Asset Asset { get; set; } = null!;

    public virtual Release Release { get; set; } = null!;
}
