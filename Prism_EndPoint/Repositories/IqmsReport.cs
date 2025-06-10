using Prism_EndPoint.Entities;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsReport
    {
        Task<ClauseReportDto> GetReportAsync(string code);
    }
}