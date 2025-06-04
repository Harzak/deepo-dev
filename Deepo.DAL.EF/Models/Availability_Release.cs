using System;
using System.Collections.Generic;

namespace Deepo.DAL.EF.Models;

public partial class Availability_Release
{
    public int Availability_Release_ID { get; set; }

    public int? Release_ID { get; set; }

    public DateTime? Availability_Date { get; set; }

    public int? Country_ID { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Release? Release { get; set; }
}
