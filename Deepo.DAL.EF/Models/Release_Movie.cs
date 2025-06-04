using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Release_Movie
{
    public int Release_Movie_ID { get; set; }

    public int Release_ID { get; set; }

    public int? Genre_Movie_ID { get; set; }

    public virtual Genre_Movie? Genre_Movie { get; set; }

    public virtual Release Release { get; set; } = null!;
}
