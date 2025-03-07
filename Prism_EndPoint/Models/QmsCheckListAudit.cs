using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsCheckListAudit
{
    public int Id { get; set; }

    public int CheckListId { get; set; }

    public int AuditPlanId { get; set; }

    public string? Questions { get; set; }

    public string? Documentation { get; set; }

    public string? Evidence { get; set; }

    public string? Findings { get; set; }

    public int ClauseId { get; set; }

    public string? Remarks { get; set; }

    public string? LookAt { get; set; }

    public virtual QmsPlanAudit AuditPlan { get; set; } = null!;

    public virtual QmsCheckList CheckList { get; set; } = null!;

    public virtual QmsSubClause Clause { get; set; } = null!;
}
