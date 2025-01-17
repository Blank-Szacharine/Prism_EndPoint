using Prism_EndPoint.Entities;

namespace Prism_EndPoint.Repositories
{
    public interface IqmsProgram
    {
        Task AddProgram();
        Task FrequencyAudit(FrequencyAuditEntities frequency);
        Task<Credential?> GetCredential();
        Task<IEnumerable<FrequencyEntities>> getFrequency(int code);
        Task<IEnumerable<ProcessEntities>> getProcess();
        Task<qmsEntitesProgram> getProgram(string code);
        Task<IEnumerable<newTeam>> getTeam();
        Task newCredential(Credential credential);
        Task newProcess(NewProcess process);
        Task newTeam(newTeam team);
        Task UpdateProgramMetho(string code, string Methodology);
        Task UpdateProgramObj(string code, string objectives);
        Task UpdateProgramScope(string code, string scope);
        Task UpdateProgramSecV(string code, string secv);
        Task UpdateProgramSecVI(string code, string secvi);
        Task UpdateProgramSecVII(string code, string secvii);
    }
}