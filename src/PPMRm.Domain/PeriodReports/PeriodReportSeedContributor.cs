using PPMRm.ARTMIS.PeriodShipments;
using PPMRm.Core;
using PPMRm.Items;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace PPMRm.PeriodReports
{
    public class PeriodReportSeedContributor : IDataSeedContributor, ITransientDependency
    {
        IRepository<PeriodReport, string> Repository { get; }
        PeriodReportManager PeriodReportManager { get; }
        IPeriodShipmentRepository ShipmentRepository { get; }
        IRepository<Country, string> CountryRepository { get; }
        IRepository<Period, int> PeriodRepository { get; }
        IRepository<Product, string> ProductRepository { get; }
        IItemRepository ItemRepository { get; }
        public PeriodReportSeedContributor(IRepository<PeriodReport, string> repository,IRepository<Country, string> countryRepository, IRepository<Product, string> productRepository, IRepository<Period, int> periodRepository, IItemRepository itemRepository, PeriodReportManager periodReportManager, IPeriodShipmentRepository shipmentRepository)
        {
            Repository = repository;
            PeriodRepository = periodRepository;
            CountryRepository = countryRepository;
            PeriodReportManager = periodReportManager;
            ItemRepository = itemRepository;
            ShipmentRepository = shipmentRepository;
            ProductRepository = productRepository;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var periods = new int[] { 202304 };
            foreach (var pId in periods)
            {
                if (await Repository.CountAsync(pr => pr.PeriodId == pId) == 0)
                {
                    var items = await ItemRepository.GetListAsync();
                    var products = await ProductRepository.GetListAsync();
                    var countries = await CountryRepository.GetListAsync();
                    var period = await PeriodRepository.GetAsync(pId);
                    var reports = await PeriodReportManager.CreateManyAsync(period, countries);
                    //await Repository.InsertManyAsync(reports);
                    foreach (var report in reports)
                    {
                        report.Open();
                        var shipment = await ShipmentRepository.GetAsync(report.CountryId, report.PeriodId);
                        var periodShipments = shipment.Shipments
                            .Where(l => l.PPMRmProductId != null && l.ShipmentDateType != ARTMIS.ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate || (l.ShipmentDate >= period.StartDate));

                        foreach (var s in periodShipments)
                        {
                            var shipmentItem = items.SingleOrDefault(i => i.Id == s.ProductId);
                            var product = products.SingleOrDefault(p => p.Id == shipmentItem?.ProductId);
                            if (product == null)
                            {
                                Console.WriteLine($"{s.ProductId} - {s.PPMRmProductId} - product not found skipping.");
                                continue;
                            }
                            var shipmentDateType = s.ShipmentDateType == ARTMIS.ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate ? ShipmentDateType.AcDD :
                                s.ShipmentDateType == ARTMIS.ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate ? ShipmentDateType.EDD :
                                ShipmentDateType.RDD;
                            var totalQuantity = s.OrderedQuantity * shipmentItem.BaseUnitMultiplier;
                            report.AddOrUpdateShipment(s.Id, product.Id, s.ShipmentDate, shipmentDateType, totalQuantity);
                        }
                    }
                    await Repository.InsertManyAsync(reports);
                }
            }
            

        }
    }
}

