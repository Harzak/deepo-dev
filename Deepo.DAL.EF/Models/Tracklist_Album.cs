using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Tracklist_Album
{
    public int Tracklist_Album_ID { get; set; }

    public int Release_Album_ID { get; set; }

    public int Track_Album_ID { get; set; }

    public virtual Release_Album Release_Album { get; set; } = null!;

    public virtual Track_Album Track_Album { get; set; } = null!;
}
