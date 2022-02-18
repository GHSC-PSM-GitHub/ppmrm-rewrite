using PPMRm.Core;
using PPMRm.ProgramReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
namespace PPMRm.PeriodReports
{
    public class PeriodReportManager : DomainService
    {
        IRepository<PeriodReport, string> PeriodReportRepository { get; }
        public PeriodReportManager(IRepository<PeriodReport, string> periodReportRepository)
        {
            PeriodReportRepository = periodReportRepository;
        }

        public async Task<PeriodReport> CreateAsync(Country country, Period period)
        {
            await Task.CompletedTask;
            Check.NotNull(country, nameof(country));
            Check.NotNull(period, nameof(period));

            var existing = PeriodReportRepository.Where(r => r.CountryId == country.Id && r.PeriodId == period.Id).SingleOrDefault();
            if (existing == null)
                return new PeriodReport(country.Id, period.Id);
            throw new ArgumentException("Period Report Already Exists");
        }

        public async Task<List<PeriodReport>> CreateManyAsync(Period period, List<Country> countries)
        {
            Check.NotNull(period, nameof(period));
            Check.NotNullOrEmpty(countries, nameof(countries));
            if (await PeriodReportRepository.AnyAsync(r => r.PeriodId == period.Id)) throw new ArgumentException("Period Reports already exist");
            return countries.Select(c => new PeriodReport(c.Id, period.Id)).ToList();
        }
    }
}
