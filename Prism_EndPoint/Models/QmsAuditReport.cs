using System;
using System.Collections.Generic;

namespace Prism_EndPoint.Models;

public partial class QmsAuditReport
{
    public int Id { get; set; }

    public int ChecklistId { get; set; }

    public string? AuditArea { get; set; }

    public string? Pgpidentified { get; set; }

    public string? NonConfirmities { get; set; }

    public string? Ofi { get; set; }

    public string? Conclusion { get; set; }

    public DateOnly? AuditorSigned { get; set; }

    public DateOnly? ViceAuditorSigned { get; set; }

    public DateOnly? AuditeeSigned { get; set; }

    public string? NextAction { get; set; }

    public virtual QmsCheckList Checklist { get; set; } = null!;
}
