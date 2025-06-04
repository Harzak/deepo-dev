using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Provider
{
    public int Provider_ID { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Provider_Release> Provider_Releases { get; set; } = new List<Provider_Release>();
}
