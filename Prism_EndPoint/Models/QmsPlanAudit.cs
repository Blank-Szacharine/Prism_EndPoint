﻿using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsPlanAudit
{
    public int Id { get; set; }

    public int PlanId { get; set; }

    public int? SubProcessId { get; set; }

    public string? AuditCriteria { get; set; }

    public string? ProcessOwner { get; set; }

    public int? TeamId { get; set; }

    public int? DivisionId { get; set; }

    public DateOnly? AuditDate { get; set; }

    public TimeOnly? TimeFrom { get; set; }

    public TimeOnly? TimeTo { get; set; }

    public virtual QmsPlan Plan { get; set; } = null!;

    public virtual QmssubProcess? SubProcess { get; set; }

    public virtual Qmsteam? Team { get; set; }
}
