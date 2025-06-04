using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Execution
{
    public int Execution_ID { get; set; }

    public int Fetcher_ID { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public virtual Fetcher Fetcher { get; set; } = null!;
}
