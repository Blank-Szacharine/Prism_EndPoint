using Microsoft.EntityFrameworkCore;
using Prism_EndPoint.Entities;
using Prism_EndPoint.Models;
using System;
using System.Diagnostics;
namespace Prism_EndPoint.Repositories
{
    public class qmsProgram : IqmsProgram
    {
        private readonly PrismDbContext _QmsContext;
        public qmsProgram(PrismDbContext QmsContext)
        {
            _QmsContext = QmsContext;
        }
        public async Task<qmsEntitesProgram> getProgram(string code)
        {
            var fetchProgram = await _QmsContext.QmsPrograms
                .Include(x => x.FrequencyAudits)
                .Where(x => x.Code == code)
                .FirstOrDefaultAsync();

            qmsEntitesProgram program = null;

            if (fetchProgram != null)
            {
                program = new qmsEntitesProgram
                {
                    ProgramId = fetchProgram.Id,
                    ProgramObj = fetchProgram.ProgramObj,
                    ProgramScope = fetchProgram.ProgramScope,
                    ProgramMethodology = fetchProgram.ProgramMethodology,
                    ProgramSecV = fetchProgram.ProgramSecV,
                    ProgramSecVI = fetchProgram.ProgramSecVi,
                    ProgramSecVII = fetchProgram.ProgramSecVii,
                    DateCreated = fetchProgram.DateCreated,
                    QMSleader = fetchProgram.Qmsleader,
                    QMSauditLead = fetchProgram.QmsauditLead,
                    ApprovedQMSLEAD =fetchProgram.ApprovedQmslead,
                    ApprovedAuditHead = fetchProgram.ApprovedAuditHead,
                    code = fetchProgram.Code,
                };
            }


            return program;
        }

        public async Task AddProgram()
        {
            var savedCredential = await _QmsContext.PrismCredentials.FirstOrDefaultAsync();
            var lastId = _QmsContext.QmsPrograms.OrderByDescending(p => p.Id).FirstOrDefault();

            var currentYear = DateTime.Now.Year;
            var idStart = 0;
            if (lastId == null)
            {
                idStart = 1;
            }
            else
            {
                idStart = lastId.Id;
            }
            var code = $"QMS-{currentYear}-{idStart + 1}";
            var newProgram = new QmsProgram
            {
                Code = code,
                DateCreated = DateOnly.FromDateTime(DateTime.Today),
                Qmsleader = savedCredential.Qmsleader,
                QmsauditLead = savedCredential.AuditHead,
                ApprovedAuditHead ="No",
                ApprovedQmslead ="No"
            };

            await _QmsContext.QmsPrograms.AddAsync(newProgram);
            await _QmsContext.SaveChangesAsync();
        }

        public async Task UpdateProgramObj(string code, string objectives)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramObj = objectives;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task UpdateProgramScope(string code, string scope)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramScope = scope;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task UpdateProgramMetho(string code, string Methodology)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramMethodology = Methodology;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task UpdateProgramSecV(string code, string secv)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramSecV = secv;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }
        public async Task UpdateProgramSecVI(string code, string secvi)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramSecVi = secvi;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }
        public async Task UpdateProgramSecVII(string code, string secvii)
        {
            var program = await _QmsContext.QmsPrograms.Where(x => x.Code == code).FirstOrDefaultAsync();

            if (program != null)
            {
                try
                {
                    program.ProgramSecVii = secvii;
                    _QmsContext.QmsPrograms.Update(program);
                    await _QmsContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error updating employee in the database", ex);
                }
            }
        }

        public async Task newProcess(NewProcess process)
        {
            try
            {
                var newProcess = new Qmsprocess
                {
                    ProcessTitle = process.title,
                    ProcessDescription = process.description,
                };
                _QmsContext.Qmsprocesses.Add(newProcess);
                await _QmsContext.SaveChangesAsync();

                foreach (var subProcess in process.SubProcess)
                {
                    var NewsubProcess = new QmssubProcess
                    {
                        ProcessId = newProcess.Id,
                        SubProcessName = subProcess.ProcessTitle
                    };
                    _QmsContext.QmssubProcesses.Add(NewsubProcess);
                    await _QmsContext.SaveChangesAsync();

                }

                var newDivisionProcess = new DivisionProcess
                {
                    ProcessOwnerId = process.owner,
                    DivisionId = process.division,
                    ProcessId = newProcess.Id
                };
                _QmsContext.DivisionProcesses.Add(newDivisionProcess);
                await _QmsContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                throw;
            }
            

        }

        public async Task<IEnumerable<ProcessEntities>> getProcess()
        {
            var Processes = await _QmsContext.DivisionProcesses
                .Include(x => x.Process)
                .ToListAsync();



            List<ProcessEntities> process = Processes.Select(e => new ProcessEntities
            {
                division = e.DivisionId,
                owner = e.ProcessOwnerId,
                title = e.Process.ProcessTitle,
                description = e.Process.ProcessDescription,
                ProcessFile = e.Process.ProcessFile,
                ProcessDocNum = e.Process.ProcessDocNum,

            }).ToList();

            return process;

        }

        public async Task newTeam(newTeam team)
        {
            var newTeam = new Qmsteam
            {
                TeamLeader = team.teamleader
            };
            _QmsContext.Qmsteams.Update(newTeam);
            await _QmsContext.SaveChangesAsync();

            foreach (var member in team.membersTeams)
            {
                var newmember = new QmsteamMember
                {
                    Member = member.memberId,
                    TeamId = newTeam.Id,
                };
                _QmsContext.QmsteamMembers.Update(newmember);
                await _QmsContext.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<newTeam>> getTeam()
        {
            try
            {
                var Team = await _QmsContext.Qmsteams
                .Include(x => x.QmsteamMembers)
                .ToListAsync();



                List<newTeam> teams = Team.Select(e => new newTeam
                {
                    teamID = e.Id,
                    teamleader = e.TeamLeader,
                    membersTeams = e.QmsteamMembers.Select(m => new newTeam.membersTeam
                    {
                        memberId = m.Member,
                        teamId = m.TeamId
                    }).ToList()
                }).ToList();

                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                throw;
            }

        }

        public async Task FrequencyAudit(FrequencyAuditEntities frequency)
        {
            try
            {
                var currentYear = DateTime.Now.Year;




                foreach (var frequencis in frequency.ArrayInputs)
                {
                    var existingFrequency = await _QmsContext.FrequencyAudits
                        .FirstOrDefaultAsync(f => f.ProgramId == frequency.codeProgram && f.DivisionId == frequencis.divisionId);

                    if (existingFrequency != null)
                    {
                        var existingFrequencyMonths = _QmsContext.FrequencyMonths
                            .Where(fm => fm.FrequencyId == existingFrequency.Id);
                        _QmsContext.FrequencyMonths.RemoveRange(existingFrequencyMonths);


                        existingFrequency.AuditDate = DateOnly.FromDateTime(DateTime.Today);
                        existingFrequency.AuditTeam = frequencis.AuditTeam;
                        _QmsContext.FrequencyAudits.Update(existingFrequency);
                        await _QmsContext.SaveChangesAsync();


                        foreach (var months in frequencis.FrequencyMonths)
                        {
                            var monthFrequency = new FrequencyMonth
                            {
                                FrequencyId = existingFrequency.Id,
                                MonthFrequency = months.month,
                            };
                            _QmsContext.FrequencyMonths.Add(monthFrequency);
                        }
                        await _QmsContext.SaveChangesAsync();
                    }
                    else
                    {
                        var saveFrequency = new FrequencyAudit
                        {
                            AuditDate = DateOnly.FromDateTime(DateTime.Today),
                            AuditTeam = frequencis.AuditTeam,
                            ProgramId = frequency.codeProgram,
                            DivisionId = frequencis.divisionId,
                        };
                        _QmsContext.FrequencyAudits.Add(saveFrequency);
                        await _QmsContext.SaveChangesAsync();

                        foreach (var months in frequencis.FrequencyMonths)
                        {
                            var monthFrequency = new FrequencyMonth
                            {
                                FrequencyId = saveFrequency.Id,
                                MonthFrequency = months.month,
                            };
                            _QmsContext.FrequencyMonths.Add(monthFrequency);
                        }
                        await _QmsContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                throw;
            }




        }
        public async Task<IEnumerable<FrequencyEntities>> getFrequency(int code)
        {
            try
            {
                var frequency = await _QmsContext.FrequencyAudits
                .Include(x => x.FrequencyMonths)
                .Where(x => x.ProgramId == code)
                .ToListAsync();


                List<FrequencyEntities> frequencies = frequency.Select(e => new FrequencyEntities
                {
                    divisionId = e.DivisionId,
                    AuditTeam = e.AuditTeam,
                    programId = e.ProgramId,
                    FrequencyMonths = e.FrequencyMonths.Select(m => new FrequencyEntities.FrequencyMonth
                    {
                        month = m.MonthFrequency
                    }).ToList(),



                }).ToList();
                return frequencies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                throw;
            }

        }

        public async Task<Credential?> GetCredential()
        {
            try
            {

                var prismCredential = await _QmsContext.PrismCredentials
                    .FirstOrDefaultAsync();

                if (prismCredential == null)
                {
                    Console.WriteLine("No credential found.");
                    return null;
                }

                Credential credential = new Credential
                {
                    Id = prismCredential.Id,
                    Qmsleader = prismCredential.Qmsleader,
                    ViceQmsleader = prismCredential.ViceQmsleader,
                    AuditHead = prismCredential.AuditHead,
                    ViceAuditHead = prismCredential.ViceAuditHead
                };

                return credential;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }


        public async Task newCredential(Credential credential)
        {
            var savedCredential = await _QmsContext.PrismCredentials.FirstOrDefaultAsync();

            if (savedCredential == null)
            {
                
                throw new InvalidOperationException("No saved credential found.");
            }


            
            if (credential.Qmsleader != null)
            {
                savedCredential.Qmsleader = credential.Qmsleader;
            }

            if (credential.ViceQmsleader != null)
            {
                savedCredential.ViceQmsleader = credential.ViceQmsleader;
            }

            if (credential.AuditHead != null)
            {
                savedCredential.AuditHead = credential.AuditHead;
            }

            if (credential.ViceAuditHead != null)
            {
                savedCredential.ViceAuditHead = credential.ViceAuditHead;
            }

            
            _QmsContext.PrismCredentials.Update(savedCredential);
            await _QmsContext.SaveChangesAsync();
        }

    }

}

