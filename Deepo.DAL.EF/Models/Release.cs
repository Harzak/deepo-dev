using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Release
{
    public int Release_ID { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Release_Date_UTC { get; set; }

    public string? Description { get; set; }

    public int Type_Release_ID { get; set; }

    public DateTime Creation_Date { get; set; }

    public DateTime Modification_Date { get; set; }

    public string? Creation_User { get; set; }

    public string? Modification_User { get; set; }

    public string GUID { get; set; } = null!;

    public virtual ICollection<Asset_Release> Asset_Releases { get; set; } = new List<Asset_Release>();

    public virtual ICollection<Author_Release> Author_Releases { get; set; } = new List<Author_Release>();

    public virtual ICollection<Availability_Release> Availability_Releases { get; set; } = new List<Availability_Release>();

    public virtual ICollection<Provider_Release> Provider_Releases { get; set; } = new List<Provider_Release>();

    public virtual ICollection<Release_Album> Release_Albums { get; set; } = new List<Release_Album>();

    public virtual ICollection<Release_Movie> Release_Movies { get; set; } = new List<Release_Movie>();

    public virtual ICollection<Release_TVShow> Release_TVShows { get; set; } = new List<Release_TVShow>();

    public virtual Type_Release Type_Release { get; set; } = null!;
}
