﻿using PPMRm.Core;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            PeriodReportRepository = repository;
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
                        join c in countries
                        on r.CountryId equals c.Id
                        join p in periods.Where(period => period.Year == input.Year.GetValueOrDefault() && period.Month == input.Month)
                        on r.PeriodId equals p.Id
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
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();
            var programsQueryable = await ProgramRepository.GetQueryableAsync();
            var productsQueryable = await ProductRepository.GetQueryableAsync();

            var query = from r in queryable.Where(i => i.Id == id)
                         join c in countries on r.CountryId equals c.Id
                         join p in periods on r.PeriodId equals p.Id
                         select new
                         {
                             PeriodReport = r,
                             Country = c,
                             Period = p
                         };
            var result = await AsyncExecuter.SingleAsync(query);
            var periodReport = result.PeriodReport;
            var programs = await ProgramRepository.ToListAsync();

            var programProductIds = periodReport.ProductShipments.GroupBy(s => new { s.ProgramId, s.ProductId }).Select(i => (i.Key.ProgramId, i.Key.ProductId))
                                               .Concat(periodReport.ProductStocks.Select(x => (x.ProgramId, x.ProductId)))
                                               .GroupBy(i => new { i.ProgramId, i.ProductId }).Select(p => (p.Key.ProgramId, p.Key.ProductId));
            
            var products = await AsyncExecuter.ToListAsync(productsQueryable.Where(p => programProductIds.Select(i => i.ProductId).Distinct().Contains(p.Id)));

            var productStockShipments = from pp in programProductIds
                                        join prog in programs on pp.ProgramId equals prog.Id
                                        join p in products on pp.ProductId equals p.Id
                                        join ps in periodReport.ProductStocks on
                                            new { pp.ProgramId, pp.ProductId } equals
                                            new { ps.ProgramId, ps.ProductId } into gStock
                                        from stock in gStock.DefaultIfEmpty()
                                   select new ProgramProductDto
                                   {
                                       Program = new ProgramDto { Id = prog.Id, Name = prog.Name },
                                       Product = new ProductDto { Id = p.Id, Name = p.Name, TracerCategory = p.TracerCategory },
                                       AMC = stock?.AMC,
                                       DateOfSOH = stock?.DateOfSOH,
                                       SOH = stock?.SOH,
                                       Shipments = periodReport.ProductShipments.Where(s => s.ProgramId == pp.ProgramId && s.ProductId == pp.ProductId)
                                                    .Select(s => new ProductShipmentDto
                                                    {
                                                        Id = s.Id,
                                                        ProductId = s.ProductId,
                                                        Quantity = s.Quantity,
                                                        AMC = stock?.AMC,
                                                        ShipmentDate = s.ShipmentDate,
                                                        DataSource = s.DataSource,
                                                        ShipmentDateType = s.ShipmentDateType,
                                                        Supplier = s.Supplier
                                                    }).ToList()
                                   };
            var response = new PeriodReportDetailDto
            {
                Id = periodReport.Id,
                CommoditySecurityUpdates = ObjectMapper.Map<CommoditySecurityUpdates, CommoditySecurityUpdatesDto>(periodReport.CommoditySecurityUpdates),
                Country = ObjectMapper.Map<Country, CountryDto>(result.Country),
                Period = ObjectMapper.Map<Period, PeriodDto>(result.Period),
                ReportStatus = periodReport.ReportStatus,
                Programs = periodReport.GetDefaultProgramIds().Select(i => new ProgramPeriodDto
                {
                    PeriodReportId = periodReport.Id,
                    ProgramId = i,
                    Program = ObjectMapper.Map<Program, ProgramDto>(programs.Single(p => p.Id == i)),
                    Products = productStockShipments.Where(s => s.Program.Id == i).OrderBy(p => p.Product.Name).ToList()
                }).ToList()
            };

            return response;
        }

        public async Task<List<ProgramProductDto>> GetDetailsAsync(string id, int programId)
        {
            var queryable = await Repository.WithDetailsAsync(r => r.ProductStocks, r => r.ProductShipments);
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();
            var programsQueryable = await ProgramRepository.GetQueryableAsync();
            var productsQueryable = await ProductRepository.GetQueryableAsync();

            var query = from r in queryable.Where(i => i.Id == id)
                        join c in countries on r.CountryId equals c.Id
                        join p in periods on r.PeriodId equals p.Id
                        select new
                        {
                            PeriodReport = r,
                            Country = c,
                            Period = p
                        };
            var result = await AsyncExecuter.SingleAsync(query);
            var periodReport = result.PeriodReport;
            var programs = await AsyncExecuter.ToListAsync(programsQueryable.Where(p => p.Id == programId));

            var programProductIds = periodReport.ProductShipments.GroupBy(s => new { s.ProgramId, s.ProductId }).Select(i => (i.Key.ProgramId, i.Key.ProductId))
                                               .Concat(periodReport.ProductStocks.Select(x => (x.ProgramId, x.ProductId)))
                                               .GroupBy(i => new { i.ProgramId, i.ProductId }).Select(p => (p.Key.ProgramId, p.Key.ProductId));

            var products = await AsyncExecuter.ToListAsync(productsQueryable.Where(p => programProductIds.Select(i => i.ProductId).Distinct().Contains(p.Id)));

            var productStockShipments = from pp in programProductIds
                                        join prog in programs on pp.ProgramId equals prog.Id
                                        join p in products on pp.ProductId equals p.Id
                                        join ps in periodReport.ProductStocks on
                                            new { pp.ProgramId, pp.ProductId } equals
                                            new { ps.ProgramId, ps.ProductId } into gStock
                                        from stock in gStock.DefaultIfEmpty()
                                        select new ProgramProductDto
                                        {
                                            Program = new ProgramDto { Id = prog.Id, Name = prog.Name },
                                            Product = new ProductDto { Id = p.Id, Name = p.Name, TracerCategory = p.TracerCategory },
                                            AMC = stock?.AMC,
                                            DateOfSOH = stock?.DateOfSOH,
                                            SOH = stock?.SOH,
                                            ActionRecommended = stock?.ActionRecommended,
                                            DateActionNeededBy = stock?.DateActionNeededBy,
                                            Shipments = periodReport.ProductShipments.Where(s => s.ProgramId == pp.ProgramId && s.ProductId == pp.ProductId)
                                                         .Select(s => new ProductShipmentDto
                                                         {
                                                             Id = s.Id,
                                                             ProductId = s.ProductId,
                                                             Quantity = s.Quantity,
                                                             AMC = stock?.AMC,
                                                             ShipmentDate = s.ShipmentDate,
                                                             DataSource = s.DataSource,
                                                             ShipmentDateType = s.ShipmentDateType,
                                                             Supplier = s.Supplier
                                                         }).ToList()
                                        };

            return productStockShipments.Where(s => s.Program.Id == programId).ToList();
        }

        async public Task UpdateCSUpdatesAsync(string id, CommoditySecurityUpdatesDto input)
        {
            var queryable = await Repository.WithDetailsAsync(r => r.CommoditySecurityUpdates);
            var periodReport = await AsyncExecuter.SingleAsync(queryable.Where(r => r.Id == id));
            var csUpdates = ObjectMapper.Map<CommoditySecurityUpdatesDto, CommoditySecurityUpdates>(input);
            periodReport.SetCSUpdates(ObjectMapper.Map<CommoditySecurityUpdatesDto, CommoditySecurityUpdates>(input));
            await PeriodReportRepository.UpdateAsync(periodReport);
        }

        async public Task<CommoditySecurityUpdatesDto> GetCSUpdatesAsync(string id)
        {
            var queryable = await Repository.WithDetailsAsync(r => r.CommoditySecurityUpdates);
            var countries = await CountryRepository.GetQueryableAsync();
            var periods = await PeriodRepository.GetQueryableAsync();
            var reports = from r in queryable.Where(r => r.Id == id)
                               join c in countries on r.CountryId equals c.Id
                               join p in periods on r.PeriodId equals p.Id
                               select new { PeriodReport = r, Country = c, Period = p };
            var periodReport = await AsyncExecuter.SingleAsync(reports);
            var title = $"{periodReport.Country.Name} - {DateTimeFormatInfo.CurrentInfo.GetMonthName(periodReport.Period.Month)} {periodReport.Period.Year}";
            CommoditySecurityUpdatesDto csUpdatesDto;
            if (periodReport.PeriodReport.CommoditySecurityUpdates != null)
                csUpdatesDto = ObjectMapper.Map<CommoditySecurityUpdates, CommoditySecurityUpdatesDto>(periodReport.PeriodReport.CommoditySecurityUpdates);
            else
                csUpdatesDto = new CommoditySecurityUpdatesDto();

            csUpdatesDto.PeriodReportId = id;
            csUpdatesDto.Title = title;
            return csUpdatesDto;
        }

        public async Task<ProgramProductDto> GetProgramProductAsync(string id, int programId, string productId)
        {
            var queryable = await PeriodReportRepository.WithDetailsAsync(r => r.ProductStocks);
            var periodReport = await AsyncExecuter.SingleAsync(queryable.Where(p => p.Id == id));
            var programProduct = periodReport.ProductStocks.SingleOrDefault(p => p.ProductId == productId && p.ProgramId == programId);
            return new ProgramProductDto
            {
                SOH = programProduct?.SOH,
                AMC = programProduct?.AMC,
                ActionRecommended = programProduct?.ActionRecommended,
                DateActionNeededBy = programProduct?.DateActionNeededBy,
                DateOfSOH = programProduct?.DateOfSOH,
            };
        }

        public async Task AddOrUpdateProgramProductAsync(string id, int programId, string productId, CreateUpdateProgramProductDto productInfo)
        {
            var queryable = await PeriodReportRepository.WithDetailsAsync(r => r.ProductStocks);
            var periodReport = await AsyncExecuter.SingleAsync(queryable.Where(p => p.Id == id));
            periodReport.AddOrUpdateProgramProduct(programId, productId, productInfo.SOHLevels, productInfo.SOH, productInfo.DateOfSOH, productInfo.AMC, productInfo.SourceOfConsumption, productInfo.ActionRecommended, productInfo.DateActionNeededBy);
            await PeriodReportRepository.UpdateAsync(periodReport);
        }

        public Task DeleteShipmentAsync(string id, Guid shipmentId)
        {
            throw new NotImplementedException();
        }

        public Task AddShipmentAsync(string id, int programId, string productId, CreateUpdateShipmentDto shipment)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShipmentAsync(string id, Guid shipmentId, CreateUpdateShipmentDto shipment)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsFinalAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task ReopenAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
