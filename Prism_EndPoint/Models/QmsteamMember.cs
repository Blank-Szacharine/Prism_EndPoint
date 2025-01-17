using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsteamMember
{
    public int Id { get; set; }

    public string? Member { get; set; }

    public int? TeamId { get; set; }

    public virtual Qmsteam? Team { get; set; }
}
