using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmssubProcess
{
    public int SubProcessId { get; set; }

    public int ProcessId { get; set; }

    public string SubProcessName { get; set; } = null!;

    public virtual Qmsprocess Process { get; set; } = null!;

    public virtual ICollection<QmsPlanAudit> QmsPlanAudits { get; set; } = new List<QmsPlanAudit>();
}
