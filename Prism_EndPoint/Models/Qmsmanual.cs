using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class Qmsmanual
{
    public int Id { get; set; }

    public int Clause { get; set; }

    public string? ClauseTitle { get; set; }

    public virtual ICollection<QmsSubClause> QmsSubClauses { get; set; } = new List<QmsSubClause>();
}
