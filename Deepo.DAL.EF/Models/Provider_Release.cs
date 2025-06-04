using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Provider_Release
{
    public int Provider_Release_ID { get; set; }

    public int Provider_ID { get; set; }

    public int Release_ID { get; set; }

    public string? Provider_Release_Identifier { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual Release Release { get; set; } = null!;
}
