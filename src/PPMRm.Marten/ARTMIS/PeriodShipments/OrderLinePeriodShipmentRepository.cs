using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityServer4.Events;
using Marten;
using PPMRm.ARTMIS.OrderLines;
using PPMRm.Core;
using PPMRm.PeriodReports;
using PPMRm.Products;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.ARTMIS.PeriodShipments
{
	public class OrderLinePeriodShipmentRepository : IPeriodShipmentRepository
	{
        IRepository<Core.Country, string> CountryRepository { get; }
        IRepository<Products.Product, string> ProductRepository { get; }
        IDocumentSession Session { get; }
        PeriodReportManager PeriodReportManager { get; }
        IRepository<Period, int> PeriodRepository { get; }

        public OrderLinePeriodShipmentRepository(
            IDocumentSession session,
            IRepository<Core.Country, string> countryRepository,
            IRepository<Products.Product, string> productRepository,
            PeriodReportManager periodReportManager,
            IRepository<Period, int> periodRepository)
        {
            Session = session;
            CountryRepository = countryRepository;
            ProductRepository = productRepository;
            PeriodReportManager = periodReportManager;
            PeriodRepository = periodRepository;
        }

        public async Task<PeriodShipment> GetAsync(string countryId, int periodId)
        {

            var periodShipmentId = $"{countryId}-{periodId}";

            var periodShipment = await Session.LoadAsync<PeriodShipment>(periodShipmentId);
            //if (periodShipment != null) return periodShipment;

            var period = await PeriodRepository.GetAsync(periodId);

            var queryable = Session.Query<OrderLine>().Where(
                l => l.CountryId == countryId &&
                l.PPMRmProductId != null &&
                (l.ActualDeliveryDate >= period.StartDate || l.ActualDeliveryDate == null));
            var results = await queryable.ToListAsync();
            var shipments = results.Select(r => new ShipmentLine
            {
                Id = r.Id,
                ProductId = r.ProductId,
                PPMRmProductId = r.PPMRmProductId,
                
                OrderedQuantity = r.OrderedQuantity,
                ShipmentDate = r.ShipmentDate,
                ShipmentDateType = r.ShipmentDateType,
            }).ToList();

            periodShipment = new PeriodShipment
            {
                Id = periodShipmentId,
                CountryId = countryId,
                PeriodId = periodId,
                Shipments = shipments
            };
            Session.Store<PeriodShipment>(periodShipment);
            await Session.SaveChangesAsync();
            return periodShipment;
        }
    }
}

