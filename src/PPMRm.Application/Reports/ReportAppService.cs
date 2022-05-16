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

        #region Old Implementation

        [Obsolete]
        public async Task<List<StockStatusDto>> GetStockStatusLegacyAsync(GetStockStatusDto request)
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

            var productShipmentsQueryable = from ps in (await PeriodReportRepository.WithDetailsAsync(x => x.ProductShipments)).SelectMany(x => x.ProductShipments)
                                            where request.ProductIds.Contains(ps.ProductId) && request.ProgramIds.Contains(ps.ProgramId)
                                            && periodReportIds.Contains(ps.PeriodReportId)
                                            select ps;
                                            

            var productShipmentsResult = from ps in productShipmentsQueryable.ToList()
                                         //join r in productStocksResult on new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } equals new { r.PeriodReportId, r.ProgramId, r.ProductId } into productShipments
                                         group ps by new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } into g
                                         select new
                                         {
                                             PeriodReportId = g.Key.PeriodReportId,
                                             ProgramId = g.Key.ProgramId,
                                             ProductId = g.Key.ProductId,
                                             Shipments = g.OrderBy(x => x.ShipmentDate?.Ticks).ToList()
                                         };


            var result = from ps in productStocksResult
                                join product in await ProductRepository.GetQueryableAsync() on ps.ProductId equals product.Id
                                join program in await ProgramRepository.GetQueryableAsync() on ps.ProgramId equals program.Id
                                join pr in periodReportsResult on ps.PeriodReportId equals pr.PeriodReport.Id
                                join ship in productShipmentsResult on new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } equals new { ship.PeriodReportId, ship.ProgramId, ship.ProductId } into shipments
                                from s in shipments.DefaultIfEmpty()
                                orderby pr.Country.Name
                                //join c in countriesQueryable on pr.Country.Id equals c.Id
                                //join p in periodsQuerable on pr.PeriodId equals p.Id
                                //join ps in periodReportsQueryable.SelectMany(x => x.ProductShipments)
                                select new StockStatusDto
                                {
                                    Country = new CountryDto { Id = pr.Country.Id, Name = pr.Country.Name},
                                    Period = new PeriodDto { Id = pr.Period.Id, StartDate = pr.Period.StartDate, EndDate = pr.Period.EndDate, Year = pr.Period.Year, Month = pr.Period.Month},
                                    Product = new ProductDto { Id = product.Id, Name = product.Name },
                                    Program = new ProgramDto { Id = program.Id, Name = program.Name},
                                    ActionRecommended = ps.ActionRecommended,
                                    AMC = ps.AMC,
                                    DateActionNeededBy = ps.DateActionNeededBy,
                                    DateOfSOH = ps.DateOfSOH,
                                    MaxStock = pr.Country.MaxStock,
                                    MinStock = pr.Country.MinStock,
                                    Shipments = s?.Shipments?.Select(x => new ShipmentSummaryDto
                                    {
                                        AMC = ps.AMC,
                                        Supplier = x.Supplier.ToString(),
                                        Quantity = x.Quantity,
                                        DataSource = x.DataSource,
                                        ShipmentDate = x.ShipmentDate,
                                        ShipmentDateType = x.ShipmentDateType,
                                        Id = x.Id
                                    }).OrderBy(x => x.ShipmentDate).ToList() ?? new List<ShipmentSummaryDto>(),
                                    SOH = ps.SOH,
                                    SOHLevels = ps.SOHLevels.ToString(),
                                    SourceOfConsumption = ps.OtherSourceOfConsumption ?? ps.SourceOfConsumption.ToString()
                                };

            return result.ToList() ?? new List<StockStatusDto>();
        }

        #endregion

        public async Task<List<StockStatusDto>> GetStockStatusAsync(GetStockStatusDto request)
        {
            var queryable = await PeriodReportRepository.WithDetailsAsync(x => x.ProductStocks, x => x.ProductShipments);
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();

            var query = from pr in queryable
                        join c in countries on pr.CountryId equals c.Id
                        join p in periods on pr.PeriodId equals p.Id
                        where p.Id >= request.StartPeriodId && p.Id <= request.EndPeriodId
                        && request.CountryIds.Contains(c.Id)
                        && pr.ProductStocks.Any(x => request.ProductIds.Contains(x.ProductId) && request.ProgramIds.Contains(x.ProgramId))
                        select new
                        {
                            PeriodReport = pr,
                            Country = c,
                            Period = p
                        };
            var results = await AsyncExecuter.ToListAsync(query);
            if (!results.Any()) return new List<StockStatusDto>();

            var products = await ProductRepository.GetListAsync(x => request.ProductIds.Contains(x.Id));
            var programs = await ProgramRepository.GetListAsync(x => request.ProgramIds.Contains(x.Id));

            var productShipments = from ps in results.SelectMany(x => x.PeriodReport.ProductShipments).Where(x => request.ProductIds.Contains(x.ProductId) && request.ProgramIds.Contains(x.ProgramId))
                                   group ps by new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } into g
                                    select new
                                    {
                                        PeriodReportId = g.Key.PeriodReportId,
                                        ProgramId = g.Key.ProgramId,
                                        ProductId = g.Key.ProductId,
                                        Shipments = g.ToList()
                                    };
            var stockQuery = from ps in results.SelectMany(x => x.PeriodReport.ProductStocks).Where(x => request.ProductIds.Contains(x.ProductId) && request.ProgramIds.Contains(x.ProgramId))
                             join pr in results on ps.PeriodReportId equals pr.PeriodReport.Id
                             join product in products on ps.ProductId equals product.Id
                             join program in programs on ps.ProgramId equals program.Id
                             join ship in productShipments on new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } equals new { ship.PeriodReportId, ship.ProgramId, ship.ProductId } into shipments
                             from s in shipments.DefaultIfEmpty()
                             select new StockStatusDto
                             {
                                 Country = new CountryDto { Id = pr.Country.Id, Name = pr.Country.Name },
                                 Period = new PeriodDto { Id = pr.Period.Id, StartDate = pr.Period.StartDate, EndDate = pr.Period.EndDate, Year = pr.Period.Year, Month = pr.Period.Month },
                                 Product = new ProductDto { Id = product.Id, Name = product.Name },
                                 Program = new ProgramDto { Id = program.Id, Name = program.Name },
                                 ActionRecommended = ps.ActionRecommended,
                                 AMC = ps.AMC,
                                 DateActionNeededBy = ps.DateActionNeededBy,
                                 DateOfSOH = ps.DateOfSOH,
                                 MaxStock = pr.Country.MaxStock,
                                 MinStock = pr.Country.MinStock,
                                 Shipments = s?.Shipments?.Select(x => new ShipmentSummaryDto
                                 {
                                     AMC = ps.AMC,
                                     Supplier = x.Supplier.ToString(),
                                     Quantity = x.Quantity,
                                     DataSource = x.DataSource,
                                     ShipmentDate = x.ShipmentDate,
                                     ShipmentDateType = x.ShipmentDateType,
                                     Id = x.Id
                                 }).ToList() ?? new List<ShipmentSummaryDto>(),
                                 SOH = ps.SOH,
                                 SOHLevels = ps.SOHLevels.ToString(),
                                 SourceOfConsumption = ps.OtherSourceOfConsumption ?? ps.SourceOfConsumption.ToString()
                             };

            return stockQuery.OrderBy(x => x.Country.Name).ThenBy(x => x.Period.Id).ThenBy(x => x.Product.Name).ThenBy(x => x.Program.Name).ToList();
        }

        public async Task<PeriodSummaryDto> GetAsync(int id)
        {
            var queryable = await PeriodReportRepository.WithDetailsAsync(x => x.ProductStocks, x => x.ProductShipments);
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();
            var products = await ProductRepository.GetListAsync();
            var programs = await ProgramRepository.GetListAsync();
            var period = await PeriodRepository.GetAsync(id);
            var query = from pr in queryable
                          join c in countries on pr.CountryId equals c.Id
                          where pr.PeriodId == id // && pr.ProductStocks.Any()
                          select new
                          {
                              PeriodReport = pr,
                              Country = c,
                              Programs = pr.GetDefaultProgramIds()
                          };
            var results = await AsyncExecuter.ToListAsync(query);

            var productShipments = from ps in results.SelectMany(x => x.PeriodReport.ProductShipments)
                                    group ps by new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } into g
                                    select new
                                    {
                                        PeriodReportId = g.Key.PeriodReportId,
                                        ProgramId = g.Key.ProgramId,
                                        ProductId = g.Key.ProductId,
                                        Shipments = g.ToList()
                                    };
            #region Program Products only (not used)
            //var programProducts = from ps in results.SelectMany(x => x.PeriodReport.ProductStocks)
            //                      join pr in results on ps.PeriodReportId equals pr.PeriodReport.Id
            //                      join product in products on ps.ProductId equals product.Id
            //                      join program in programs on ps.ProgramId equals program.Id
            //                      join ship in productShipments on new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } equals new { ship.PeriodReportId, ship.ProgramId, ship.ProductId } into shipments
            //                      from s in shipments.DefaultIfEmpty()
            //                      select new ProgramProductDto
            //                      {
            //                          Product = new ProductDto { Id = product.Id, Name = product.Name },
            //                          ActionRecommended = ps.ActionRecommended,
            //                          AMC = ps.AMC,
            //                          DateActionNeededBy = ps.DateActionNeededBy,
            //                          DateOfSOH = ps.DateOfSOH,
            //                          MaxStock = pr.Country.MaxStock,
            //                          MinStock = pr.Country.MinStock,
            //                          OtherSourceOfConsumption = ps.OtherSourceOfConsumption,
            //                          Program = new ProgramDto { Id = program.Id, Name = program.Name},
            //                          ReportStatus = pr.PeriodReport.ReportStatus.GetValueOrDefault(),
            //                          Shipments = s?.Shipments?.Select(x => new ProductShipmentDto
            //                          {
            //                              AMC = ps.AMC,
            //                              Supplier = x.Supplier,
            //                              Quantity = x.Quantity,
            //                              DataSource = x.DataSource,
            //                              ShipmentDate = x.ShipmentDate,
            //                              ShipmentDateType = x.ShipmentDateType,
            //                              Id = x.Id
            //                          }).OrderBy(x => x.ShipmentDate).ToList() ?? new List<ProductShipmentDto>(),
            //                          SOH = ps.SOH,
            //                          SourceOfConsumption = ps.SourceOfConsumption
            //                      };
            #endregion

            var countrySummaries = results.Select(r => new CountrySummaryDto
            {
                Country = new CountryDto { Id = r.Country.Id, Name = r.Country.Name, MinStock = r.Country.MinStock, MaxStock = r.Country.MaxStock},
                Programs = (from p in r.Programs
                            join program in programs on p equals program.Id
                            select new ProgramDto { Id = p, Name = program.Name }).ToList(),
                CSUpdates = ObjectMapper.Map<CommoditySecurityUpdates, CommoditySecurityUpdatesDto>(r.PeriodReport.CommoditySecurityUpdates),
                Products = (from ps in r.PeriodReport.ProductStocks
                            join pr in results on ps.PeriodReportId equals pr.PeriodReport.Id
                            join product in products on ps.ProductId equals product.Id
                            join program in programs on ps.ProgramId equals program.Id
                            join ship in productShipments on new { ps.PeriodReportId, ps.ProgramId, ps.ProductId } equals new { ship.PeriodReportId, ship.ProgramId, ship.ProductId } into shipments
                            from s in shipments.DefaultIfEmpty()
                            select new ProgramProductDto
                           {
                               Product = new ProductDto { Id = product.Id, Name = product.Name },
                               ActionRecommended = ps.ActionRecommended,
                               AMC = ps.AMC,
                               DateActionNeededBy = ps.DateActionNeededBy,
                               DateOfSOH = ps.DateOfSOH,
                               MaxStock = pr.Country.MaxStock,
                               MinStock = pr.Country.MinStock,
                               OtherSourceOfConsumption = ps.OtherSourceOfConsumption,
                               Program = new ProgramDto { Id = program.Id, Name = program.Name },
                               ReportStatus = pr.PeriodReport.ReportStatus.GetValueOrDefault(),
                               Shipments = s?.Shipments?.Select(x => new ProductShipmentDto
                               {
                                   AMC = ps.AMC,
                                   Supplier = x.Supplier,
                                   Quantity = x.Quantity,
                                   DataSource = x.DataSource,
                                   ShipmentDate = x.ShipmentDate,
                                   ShipmentDateType = x.ShipmentDateType,
                                   Id = x.Id
                               }).OrderBy(x => x.ShipmentDate).ToList() ?? new List<ProductShipmentDto>(),
                               SOH = ps.SOH,
                               SourceOfConsumption = ps.SourceOfConsumption
                           }).ToList()
            });
            return new PeriodSummaryDto
            {
                CountrySummaries = countrySummaries.ToList(),
                Period = new PeriodDto { Id = period.Id, StartDate = period.StartDate, EndDate = period.EndDate, Month = period.Month, Year = period.Year}
            };
        }
    }
}
