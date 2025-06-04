using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Release_TVShow
{
    public int Release_TVShow_ID { get; set; }

    public int? Release_ID { get; set; }

    public string? Season { get; set; }

    public int? Genre_TVShow_ID { get; set; }

    public virtual Genre_TVShow? Genre_TVShow { get; set; }

    public virtual Release? Release { get; set; }
}
