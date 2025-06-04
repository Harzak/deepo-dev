using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Genre_TVShow
{
    public int Genre_TVShow_ID { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Release_TVShow> Release_TVShows { get; set; } = new List<Release_TVShow>();
}
