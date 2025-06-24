using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class V_LastVinylRelease
{
    public int? Release_ID { get; set; }

    public string? ReleasGUID { get; set; }

    public string? AlbumName { get; set; }

    public string? ArtistsNames { get; set; }

    public string? GenresIdentifier { get; set; }

    public DateTime? Creation_Date { get; set; }

    public string? Thumb_URL { get; set; }

    public string? Cover_URL { get; set; }
}
