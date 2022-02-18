using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.ProgramReports
{
    public class ProgramReport : FullAuditedAggregateRoot<Guid>
    {
        public ProgramReport()
        {

        }

        internal ProgramReport(Guid id, string periodReportId, Core.Programs program)
        {
            Check.NotNullOrEmpty(periodReportId, nameof(periodReportId));
            Id = id;
            PeriodReportId = periodReportId;
            ProgramId = (int)program;
            Shipments = new List<ProgramShipment>();
            ProductStocks = new List<ProductStock>();
        }

        public string PeriodReportId { get; private set; }
        public int ProgramId { get; private set; }
        public ICollection<ProgramShipment> Shipments { get; set; }
        public ICollection<ProductStock> ProductStocks { get; set; }
        public void AddShipment(Guid id, Supplier supplier, string productId, decimal quantity, DateTime? shipmentDate, ShipmentDateType shipmentDateType, ShipmentDataSource dataSource)
        {
            Shipments.Add(new ProgramShipment(id, Id, supplier, productId, quantity,shipmentDate, shipmentDateType, dataSource));
        }
        public void RemoveShipment(Guid id) => Shipments.RemoveAll(s => s.Id == id);

        public void AddOrUpdateProductInformation(string productId, decimal soh, DateTime? dateOfSoH, decimal? amc, string actionRecommended, DateTime? dateActionNeededBy)
        {
            var productStock = ProductStocks.FirstOrDefault(p => p.ProductId == productId);
            if (productStock == null)
            {
                productStock = new ProductStock(Id, productId);
                ProductStocks.Add(productStock);
            }
            productStock.SOH = soh;
            productStock.DateOfSOH = dateOfSoH;
            productStock.AMC = amc;
            productStock.ActionRecommended = actionRecommended;
            productStock.DateActionNeededBy = dateActionNeededBy;
        }

        public void RemoveProductInformation(string productId) => ProductStocks.RemoveAll(p => p.ProductId == productId);
    }
}
