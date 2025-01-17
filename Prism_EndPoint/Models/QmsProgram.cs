using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsProgram
{
    public int Id { get; set; }

    public DateOnly DateCreated { get; set; }

    public string? ProgramObj { get; set; }

    public string? ProgramScope { get; set; }

    public string? ProgramMethodology { get; set; }

    public string? ProgramSecV { get; set; }

    public string? ProgramSecVi { get; set; }

    public string? ProgramSecVii { get; set; }

    public string? Qmsleader { get; set; }

    public string? QmsauditLead { get; set; }

    public string? Code { get; set; }

    public string? DocumentNum { get; set; }

    public string? ApprovedQmslead { get; set; }

    public string? ApprovedAuditHead { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<FrequencyAudit> FrequencyAudits { get; set; } = new List<FrequencyAudit>();
}
