using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class DivisionProcess
{
    public int Id { get; set; }

    public string? ProcessOwnerId { get; set; }

    public string? DivisionId { get; set; }

    public int? ProcessId { get; set; }

    public virtual Qmsprocess? Process { get; set; }
}
