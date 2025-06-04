using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Fetcher
{
    public int Fetcher_ID { get; set; }

    public string Fetcher_GUID { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Execution> Executions { get; set; } = new List<Execution>();

    public virtual ICollection<Planification> Planifications { get; set; } = new List<Planification>();
}
