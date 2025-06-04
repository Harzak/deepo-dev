using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class PlanificationType
{
    public int PlanificationType_ID { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Planification> Planifications { get; set; } = new List<Planification>();
}
