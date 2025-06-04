using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_CompletedFetcher
{
    public string Name { get; set; } = null!;

    public string Fetcher_GUID { get; set; } = null!;

    public DateOnly? StartTime { get; set; }

    public DateOnly EndTime { get; set; }
}
