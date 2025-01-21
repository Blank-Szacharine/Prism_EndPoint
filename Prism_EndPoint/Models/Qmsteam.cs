using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class Qmsteam
{
    public int Id { get; set; }

    public string? TeamLeader { get; set; }

    public virtual ICollection<FrequencyAudit> FrequencyAudits { get; set; } = new List<FrequencyAudit>();

    public virtual ICollection<QmsPlanAudit> QmsPlanAudits { get; set; } = new List<QmsPlanAudit>();

    public virtual ICollection<QmsteamMember> QmsteamMembers { get; set; } = new List<QmsteamMember>();
}
