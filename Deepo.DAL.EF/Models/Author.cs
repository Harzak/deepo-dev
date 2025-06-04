using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Author
{
    public int Author_ID { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int Provider_ID { get; set; }

    public string Provider_Author_Identifier { get; set; } = null!;

    public virtual ICollection<Author_Release> Author_Releases { get; set; } = new List<Author_Release>();

    public virtual Provider Provider { get; set; } = null!;
}
