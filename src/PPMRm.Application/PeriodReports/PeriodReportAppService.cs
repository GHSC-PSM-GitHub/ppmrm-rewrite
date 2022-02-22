using PPMRm.Core;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.PeriodReports
{
    public class PeriodReportAppService :  
        ReadOnlyAppService<PeriodReport, PeriodReportDetailDto, PeriodReportDto, string, GetPeriodReportDto>,
        IPeriodReportAppService
    {
        IRepository<PeriodReport, string> PeriodReportRepository { get; }
        IRepository<Product, string> ProductRepository { get; }
        IRepository<Period, int> PeriodRepository { get; }
        IRepository<Program, int> ProgramRepository { get; }
        IRepository<Country, string> CountryRepository { get; }
        public PeriodReportAppService(IRepository<PeriodReport, string> repository, IRepository<Program,int> programRepository, IRepository<Country, string> countryRepository, IRepository<Period, int> periodRepository, IRepository<Product, string> productRepository) : base(repository)
        {
            ProductRepository = productRepository;
            ProgramRepository = programRepository;
            PeriodRepository = periodRepository;
            CountryRepository = countryRepository;
        }
        //public async Task<PeriodReportDetailDto> GetAsync(string countryId, int period)
        //{
        //    return await GetAsync($"{countryId}-{period}");
        //}

        public override async Task<PagedResultDto<PeriodReportDto>> GetListAsync(GetPeriodReportDto input)
        {
            var queryable = await Repository.WithDetailsAsync(r => r.ProductShipments, r => r.ProductStocks);
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();


            queryable = queryable.Where(r => input.Countries.Contains(r.CountryId));

            var query = from r in queryable
                        join c in countries on r.CountryId equals c.Id
                        join p in periods on r.PeriodId equals p.Id
                        select new PeriodReportDto
                        {
                            Id = r.Id,
                            Country = new CountryDto { Id = c.Id, Name = c.Name},
                            Period = new PeriodDto { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate},
                            CommoditySecurityUpdates = new CommoditySecurityUpdatesDto(),
                            ReportStatus = r.ReportStatus,
                            ShipmentsCount = r.ProductShipments.Count,
                            ProductsCount = r.ProductStocks.Count
                        };
            // Filter

            var totalCount = query.Count();
            var results = await AsyncExecuter.ToListAsync(query.OrderBy(r => r.Country.Name).ThenByDescending(r => r.Period.Id).Skip(input.SkipCount).Take(input.MaxResultCount));
            return new PagedResultDto<PeriodReportDto>(totalCount, results);
        }

        async public override Task<PeriodReportDetailDto> GetAsync(string id)
        {
            var queryable = await Repository.WithDetailsAsync(r => r.ProductStocks, r => r.ProductShipments);
            var periodReport = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(r => r.Id == id));
            return await base.GetAsync(id);
        }
    }
}
