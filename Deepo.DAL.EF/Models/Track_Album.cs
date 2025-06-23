using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Track_Album
{
    public int Track_Album_ID { get; set; }

    public int Position { get; set; }

    public string Title { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public virtual ICollection<Tracklist_Album> Tracklist_Albums { get; set; } = new List<Tracklist_Album>();
}
