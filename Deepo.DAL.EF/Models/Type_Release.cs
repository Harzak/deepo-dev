using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Type_Release
{
    public int Type_Release_ID { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Release> Releases { get; set; } = new List<Release>();
}
