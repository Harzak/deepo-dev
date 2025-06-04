using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_InProgressFetcher
{
    public string Name { get; set; } = null!;

    public string Fetcher_GUID { get; set; } = null!;

    public DateOnly? StartTime { get; set; }
}
