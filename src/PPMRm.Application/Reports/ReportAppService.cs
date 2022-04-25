using PPMRm.Core;
using PPMRm.PeriodReports;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace PPMRm.Reports
{
    public class ReportAppService : ApplicationService, IReportAppService
    {
        ICountryRepository CountryRepository { get; }
        IRepository<Program, int> ProgramRepository { get; }
        IRepository<Period, int> PeriodRepository { get; }
        IRepository<Product, string> ProductRepository { get; }

        IRepository<PeriodReport, string> PeriodReportRepository { get; }
        IAsyncQueryableExecuter AsyncExecuter { get; }

        public ReportAppService(ICountryRepository countryRepository, 
            IRepository<Program, int> programRepository, 
            IRepository<Period, int> periodRepository, 
            IRepository<Product, string> productRepository, 
            IRepository<PeriodReport, string> periodReportRepository,
            IAsyncQueryableExecuter asyncExecuter)
        {
            CountryRepository = countryRepository;
            ProgramRepository = programRepository;
            PeriodRepository = periodRepository;
            ProductRepository = productRepository;
            PeriodReportRepository = periodReportRepository;
            AsyncExecuter = asyncExecuter;
        }
        public async Task<List<StockStatusDto>> GetStockStatusAsync(GetStockStatusDto request)
        {
            var periodReportsQueryable = await PeriodReportRepository.WithDetailsAsync(r => r.ProductStocks, r => r.ProductShipments);
            var countriesQueryable = (await CountryRepository.GetUserCountriesAsync()).Where(x => request.CountryIds.Contains(x.Id));
            var periodsQuerable = PeriodRepository.Where(x => x.Id >= request.StartPeriodId && x.Id <= request.EndPeriodId);
            var productsQueryable = ProductRepository.Where(x => request.ProductIds.Contains(x.Id));
            var programsQueryable = ProgramRepository.Where(x => request.ProgramIds.Contains(x.Id));

            var periodReports = from pr in PeriodReportRepository.AsQueryable()
                                join c in CountryRepository.AsQueryable() on pr.CountryId equals c.Id
                                join p in PeriodRepository.AsQueryable() on pr.PeriodId equals p.Id
                                where p.Id >= request.StartPeriodId && p.Id <= request.EndPeriodId
                                && request.CountryIds.Contains(c.Id)
                                select new
                                {
                                    PeriodReport = pr,
                                    Country = c,
                                    Period = p
                                };

            var periodReportIdsQueryable = from pr in PeriodReportRepository.AsQueryable()
                                           where request.StartPeriodId <= pr.PeriodId && request.EndPeriodId >= pr.PeriodId
                                           && request.CountryIds.Contains(pr.CountryId)
                                           select pr.Id;

                                  

            var periodReportIds = await AsyncExecuter.ToListAsync(periodReportIdsQueryable);

            var periodReportsResult = await AsyncExecuter.ToListAsync(periodReports);

            var productStocksQueryable = from ps in (await PeriodReportRepository.WithDetailsAsync(x => x.ProductStocks)).SelectMany(x => x.ProductStocks)
                                where request.ProductIds.Contains(ps.ProductId) && request.ProgramIds.Contains(ps.ProgramId)
                                && periodReportIds.Contains(ps.PeriodReportId)
                                select ps;

            var productStocksResult = productStocksQueryable.ToList();



            var result = from ps in productStocksResult
                                join product in await ProductRepository.GetQueryableAsync() on ps.ProductId equals product.Id
                                join program in await ProgramRepository.GetQueryableAsync() on ps.ProgramId equals program.Id
                                join pr in periodReportsResult on ps.PeriodReportId equals pr.PeriodReport.Id
                                where request.ProgramIds.Contains(program.Id) && request.ProductIds.Contains(product.Id)
                                orderby pr.Country.Name
                                //join c in countriesQueryable on pr.Country.Id equals c.Id
                                //join p in periodsQuerable on pr.PeriodId equals p.Id
                                //join ps in periodReportsQueryable.SelectMany(x => x.ProductShipments)
                                select new StockStatusDto
                                {
                                    Country = new CountryDto { Id = pr.Country.Id, Name = pr.Country.Name},
                                    Period = new PeriodDto { Id = pr.Period.Id, StartDate = pr.Period.StartDate, EndDate = pr.Period.EndDate},
                                    Product = new ProductDto { Id = product.Id, Name = product.Name },
                                    Program = new ProgramDto { Id = program.Id, Name = program.Name},
                                    ActionRecommended = ps.ActionRecommended,
                                    AMC = ps.AMC,
                                    DateActionNeededBy = ps.DateActionNeededBy,
                                    DateOfSOH = ps.DateOfSOH,
                                    MaxStock = pr.Country.MaxStock,
                                    MinStock = pr.Country.MinStock,
                                    //Shipments = new List<ShipmentSummaryDto>(),
                                    SOH = ps.SOH,
                                    //SOHLevels = ps.SOHLevels.ToString(),
                                    //SourceOfConsumption = ps.SourceOfConsumption == ,
                                    
                                };

            return result.ToList();
        }
    }
}
