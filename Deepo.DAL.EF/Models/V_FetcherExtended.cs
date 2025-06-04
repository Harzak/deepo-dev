using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_FetcherExtended
{
    public string Fetcher_GUID { get; set; } = null!;

    public string FetcherName { get; set; } = null!;

    public DateTime? DateNextStart { get; set; }

    public string? PlanificationTypeName { get; set; }

    public string? Code { get; set; }

    public int? HourStart { get; set; }

    public int? HourEnd { get; set; }

    public int? MinuteStart { get; set; }

    public int? MinuteEnd { get; set; }

    public DateTime? LastStart { get; set; }

    public DateTime? LastEnd { get; set; }
}
