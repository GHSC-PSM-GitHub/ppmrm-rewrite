using System;
using Volo.Abp.Domain.Entities;

namespace PPMRm.PeriodReports
{
    public class ProductShipment : Entity<Guid>
    {
        private ProductShipment()
        {

        }

        internal ProductShipment(Guid id, string periodReportId, int programId, string productId, Supplier supplier, DateTime? shipmentDate, ShipmentDateType shipmentDateType, decimal quantity, ShipmentDataSource shipmentDataSource) : base(id)
        {
            PeriodReportId = periodReportId;
            ProgramId = programId;
            ProductId = productId;
            Supplier = supplier;
            ShipmentDate = shipmentDate;
            ShipmentDateType = shipmentDateType;
            Quantity = quantity;
            DataSource = shipmentDataSource;
        }
        public string PeriodReportId { get; protected set; }
        public int ProgramId { get; protected set; }
        public Supplier Supplier { get; protected set; }
        public DateTime? ShipmentDate { get; protected set; }
        public ShipmentDateType ShipmentDateType { get; protected set; }
        public string ProductId { get; protected set; }
        /// <summary>
        /// (Optional) ARTMIS Item/Product ID
        /// </summary>
        public string ItemId { get; protected set; }
        public decimal Quantity { get; protected set; }
        public ShipmentDataSource DataSource { get; protected set; }

        internal static ProductShipment CreateARTMISShipment(Guid id, string periodReportId, int programId, string productId, DateTime? shipmentDate, ShipmentDateType shipmentDateType, decimal quantity, string psmProductId = null)
        {
            return new ProductShipment(id, periodReportId, programId, productId, Supplier.PMI, shipmentDate, shipmentDateType, quantity, ShipmentDataSource.ARTMIS)
            {
                ItemId = psmProductId
            };
        }
    }
}
