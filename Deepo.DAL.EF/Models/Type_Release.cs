using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Type_Release
{
    public int Type_Release_ID { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Release_Fetch_History> Release_Fetch_Histories { get; set; } = new List<Release_Fetch_History>();

    public virtual ICollection<Release> Releases { get; set; } = new List<Release>();
}
