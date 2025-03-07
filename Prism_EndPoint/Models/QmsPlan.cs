using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsPlan
{
    public int Id { get; set; }

    public int ProgramId { get; set; }

    public string? DocumentNumber { get; set; }

    public int? ProcessAudit { get; set; }

    public string? AuditObj { get; set; }

    public string? AuditMemo { get; set; }

    public string? AuditScope { get; set; }

    public int? DivisionId { get; set; }

    public string? Status { get; set; }

    public string? Approve { get; set; }

    public int? ProcessId { get; set; }

    public int? FrequencyId { get; set; }

    public string? Notes { get; set; }

    public string? NotesBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateOnly? ApprovedDateTl { get; set; }

    public DateOnly? ApprovedDateIqa { get; set; }

    public DateOnly? ApprovedDateQms { get; set; }

    public virtual FrequencyAudit? Frequency { get; set; }

    public virtual Qmsprocess? Process { get; set; }

    public virtual QmsProgram Program { get; set; } = null!;

    public virtual ICollection<QmsCheckList> QmsCheckLists { get; set; } = new List<QmsCheckList>();

    public virtual ICollection<QmsPlanAudit> QmsPlanAudits { get; set; } = new List<QmsPlanAudit>();
}
