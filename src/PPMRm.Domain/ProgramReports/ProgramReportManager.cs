using PPMRm.Core;
using PPMRm.PeriodReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PPMRm.ProgramReports
{
    public class ProgramReportManager : DomainService
    {
        const string Uganda = "UGA";
        const string Myanmar = "MMR";
        IRepository<ProgramReport, Guid> ProgramReportRepository { get; }
        IRepository<PeriodReport, string> PeriodReportRepository { get; }
        public ProgramReportManager(IRepository<ProgramReport, Guid> programReportRepository, IRepository<PeriodReport, string> periodReportRepository)
        {
            ProgramReportRepository = programReportRepository;
            PeriodReportRepository = periodReportRepository;
        }

        public async Task<List<ProgramReport>> CreateManyAsync(PeriodReport periodReport)
        {
            Check.NotNull(periodReport, nameof(periodReport));
            if (await ProgramReportRepository.AnyAsync(r => r.PeriodReportId == periodReport.Id)) throw new ArgumentException("Program Reports already exist");

            var shipments = new List<ProgramShipment>(); //Get from ARTMIS Tables

            if (periodReport.CountryId == Uganda)
            {
                var publicSector = await CreateAsync(periodReport, Programs.PublicSector);
                var pnfpSector = await CreateAsync(periodReport, Programs.PNFP);
                return new List<ProgramReport> { publicSector, pnfpSector };
            }
            else if (periodReport.CountryId == Myanmar)
            {
                var capMalaria = await CreateAsync(periodReport, Programs.CAPMalaria);
                var nationalMalariaProgram = await CreateAsync(periodReport, Programs.NationalMalariaProgram);
                return new List<ProgramReport> { capMalaria, nationalMalariaProgram };
            }
            else
            {
                var nationalMalariaProgram = await CreateAsync(periodReport, Programs.NationalMalariaProgram);
                return new List<ProgramReport> { nationalMalariaProgram };
            }
        }

        public async Task<ProgramReport> CreateAsync(PeriodReport periodReport, Programs program)
        {
            Check.NotNull(periodReport, nameof(periodReport.Id));
            if (await ProgramReportRepository.AnyAsync(r => r.PeriodReportId == periodReport.Id && r.ProgramId == (int)program)) throw new ArgumentException("");
            return new ProgramReport(GuidGenerator.Create(), periodReport.Id, program);
        }
    }
}
