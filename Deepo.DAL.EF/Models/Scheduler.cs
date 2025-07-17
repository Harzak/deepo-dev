using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Scheduler
{
    public int Scheduler_ID { get; set; }

    public int? Schedule_ID { get; set; }

    public int Fetcher_ID { get; set; }

    public virtual Fetcher Fetcher { get; set; } = null!;

    public virtual Schedule? Schedule { get; set; }
}
