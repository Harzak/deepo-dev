using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_FetchersLastExecution
{
    public string Fetcher_GUID { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }
}
