using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class Qmsprocess
{
    public int Id { get; set; }

    public string? ProcessTitle { get; set; }

    public string? ProcessDescription { get; set; }

    public string? ProcessFile { get; set; }

    public string? ProcessDocNum { get; set; }

    public virtual ICollection<DivisionProcess> DivisionProcesses { get; set; } = new List<DivisionProcess>();
}
