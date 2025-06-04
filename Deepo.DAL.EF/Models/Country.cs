using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Country
{
    public int Country_ID { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Availability_Release> Availability_Releases { get; set; } = new List<Availability_Release>();
}
