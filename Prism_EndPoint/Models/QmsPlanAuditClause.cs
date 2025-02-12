using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsPlanAuditClause
{
    public int Id { get; set; }

    public string? SubClause { get; set; }

    public int PlanAuditId { get; set; }

    public virtual QmsPlanAudit PlanAudit { get; set; } = null!;
}
