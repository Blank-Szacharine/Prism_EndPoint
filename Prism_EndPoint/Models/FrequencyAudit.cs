using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class FrequencyAudit
{
    public int Id { get; set; }

    public DateOnly AuditDate { get; set; }

    public int? AuditTeam { get; set; }

    public int? ProgramId { get; set; }

    public int? DivisionId { get; set; }

    public int? ProcessId { get; set; }

    public virtual Qmsteam? AuditTeamNavigation { get; set; }

    public virtual ICollection<FrequencyMonth> FrequencyMonths { get; set; } = new List<FrequencyMonth>();

    public virtual Qmsprocess? Process { get; set; }

    public virtual QmsProgram? Program { get; set; }

    public virtual ICollection<QmsPlan> QmsPlans { get; set; } = new List<QmsPlan>();
}
