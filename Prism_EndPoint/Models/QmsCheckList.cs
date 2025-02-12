using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsCheckList
{
    public int Id { get; set; }

    public int AuditId { get; set; }

    public string? AuditedBy { get; set; }

    public DateOnly? AuditedDate { get; set; }

    public string? AcknowledgeBy { get; set; }

    public DateOnly? AcknowledgeDate { get; set; }

    public string? Status { get; set; }

    public virtual QmsPlan Audit { get; set; } = null!;

    public virtual ICollection<QmsAuditReport> QmsAuditReports { get; set; } = new List<QmsAuditReport>();

    public virtual ICollection<QmsCheckListAudit> QmsCheckListAudits { get; set; } = new List<QmsCheckListAudit>();
}
