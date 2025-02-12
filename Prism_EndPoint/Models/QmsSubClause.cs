using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsSubClause
{
    public int Id { get; set; }

    public int ClauseId { get; set; }

    public string? Subclause { get; set; }

    public string? ClauseTitle { get; set; }

    public string? SubTitle { get; set; }

    public string? NewColumn { get; set; }

    public virtual Qmsmanual Clause { get; set; } = null!;

    public virtual ICollection<QmsCheckListAudit> QmsCheckListAudits { get; set; } = new List<QmsCheckListAudit>();

    public virtual ICollection<QmsPlanAudit> QmsPlanAudits { get; set; } = new List<QmsPlanAudit>();
}
