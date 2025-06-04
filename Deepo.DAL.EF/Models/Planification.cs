using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Planification
{
    public int Planification_ID { get; set; }

    public int? PlanificationType_ID { get; set; }

    public int? Planning_ID { get; set; }

    public int Fetcher_ID { get; set; }

    public DateTime? DateNextStart { get; set; }

    public virtual Fetcher Fetcher { get; set; } = null!;

    public virtual PlanificationType? PlanificationType { get; set; }

    public virtual Planning? Planning { get; set; }
}
