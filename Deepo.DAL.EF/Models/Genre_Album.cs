using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Genre_Album
{
    public int Genre_Album_ID { get; set; }

    public string Identifier { get; set; } = string.Empty;

    public string Name { get; set; } = null!;

    public virtual ICollection<Release_Album> Release_Albums { get; set; } = [];
}
