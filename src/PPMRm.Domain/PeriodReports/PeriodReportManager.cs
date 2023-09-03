using PPMRm.Core;
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
        IRepository<Period, int> PeriodRepository { get; }
        public PeriodReportManager(IRepository<PeriodReport, string> periodReportRepository, IRepository<Period, int> periodRepository)
        {
            PeriodReportRepository = periodReportRepository;
            PeriodRepository = periodRepository;
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

        public async Task<List<PeriodReport>> GetPeriodReportsAsync(int periodId)
        {
            var queryable = await PeriodReportRepository.WithDetailsAsync(r => r.ProductShipments);
            return await AsyncExecuter.ToListAsync(queryable.Where(r => r.PeriodId == periodId));
        }

        public async Task<Period> GetCurrentPeriodAsync()
        {
            var queryable = PeriodReportRepository.OrderByDescending(x => x.PeriodId);
            var lastReport = await AsyncExecuter.FirstOrDefaultAsync(queryable);
            return await PeriodRepository.GetAsync(lastReport.PeriodId);
        }

        public async Task<Period> GetNextPeriodAsync()
        {
            var lastPeriod = await GetCurrentPeriodAsync();
            var nextPeriod = PeriodRepository.OrderBy(x => x.Id);
            return await AsyncExecuter.FirstOrDefaultAsync(nextPeriod.Where(p => p.Id > lastPeriod.Id));

        }
    }
}
