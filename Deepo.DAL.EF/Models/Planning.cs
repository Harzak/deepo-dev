using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Planning
{
    public int Planing_ID { get; set; }

    public int? HourStart { get; set; }

    public int? MinuteStart { get; set; }

    public int? HourEnd { get; set; }

    public int? MinuteEnd { get; set; }

    public virtual ICollection<Planification> Planifications { get; set; } = new List<Planification>();
}
