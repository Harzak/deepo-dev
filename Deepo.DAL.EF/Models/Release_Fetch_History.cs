using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Release_Fetch_History
{
    public int Release_Fetch_History_ID { get; set; }

    public DateTime Date_UTC { get; set; }

    public string Identifier { get; set; } = null!;

    public string? Identifier_Desc { get; set; }

    public int Type_Release_ID { get; set; }

    public int Provider_ID { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual Type_Release Type_Release { get; set; } = null!;
}
