using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class PrismCredential
{
    public int Id { get; set; }

    public string? Qmsleader { get; set; }

    public string? ViceQmsleader { get; set; }

    public string? AuditHead { get; set; }

    public string? ViceAuditHead { get; set; }
}
