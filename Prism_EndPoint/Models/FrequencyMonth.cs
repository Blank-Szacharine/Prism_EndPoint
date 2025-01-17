using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class FrequencyMonth
{
    public int Id { get; set; }

    public int? FrequencyId { get; set; }

    public int? MonthFrequency { get; set; }

    public virtual FrequencyAudit? Frequency { get; set; }
}
