using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Schedule
{
    public int Schedule_ID { get; set; }

    public string? CronExpression { get; set; }

    public virtual ICollection<Scheduler> Schedulers { get; set; } = new List<Scheduler>();
}
