using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_LastVinylRelease
{
    public int Release_ID { get; set; }

    public DateTime Release_Date_UTC { get; set; }

    public string ReleasGUID { get; set; } = null!;

    public string AlbumName { get; set; } = null!;

    public string? ArtistsNames { get; set; }

    public string? Market { get; set; }

    public string? GenresIdentifier { get; set; }

    public DateTime Creation_Date { get; set; }

    public string? Thumb_URL { get; set; }

    public string? Cover_URL { get; set; }
}
