using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_Spotify_Vinyl_Fetch_History
{
    public DateTime Date_UTC { get; set; }

    public string Identifier { get; set; } = null!;
}
