using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsChecklist
    {
        Task createCheckList(int code);
        Task<getChecklist> getCheckListItem(int code);
        Task<ReportEntities> GetChecklistReport(int code);
        Task updateCheckListData(updateCheckList update);
        Task updateReport(ReportUpdateData update);
        Task updateStatus(int id);
    }
}