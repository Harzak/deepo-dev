﻿using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Release_Album
{
    public int Release_Album_ID { get; set; }

    public int Release_ID { get; set; }

    public string? Duration { get; set; }

    public string? Label { get; set; }

    public string? Country { get; set; }

    public string? Market { get; set; }

    public virtual Release Release { get; set; } = null!;

    public virtual ICollection<Tracklist_Album> Tracklist_Albums { get; set; } = new List<Tracklist_Album>();

    public virtual ICollection<Genre_Album> Genre_Albums { get; set; } = new List<Genre_Album>();
}
