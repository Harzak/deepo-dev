using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Author_Release
{
    public int Author_Release_ID { get; set; }

    public int Release_ID { get; set; }

    public int Author_ID { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Release Release { get; set; } = null!;
}
