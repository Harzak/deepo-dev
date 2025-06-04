using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Genre_Movie
{
    public int Genre_Movie_ID { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Release_Movie> Release_Movies { get; set; } = new List<Release_Movie>();
}
