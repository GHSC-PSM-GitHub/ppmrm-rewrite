using System;
using Volo.Abp.Domain.Entities;

namespace PPMRm.PeriodReports
{
    public class ProductShipment : Entity<Guid>
    {
        public string PeriodReportId { get; set; }
        public int ProgramId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
        public string ProductId { get; set; }
        /// <summary>
        /// (Optional) ARTMIS Item/Product ID
        /// </summary>
        public string ItemId { get; set; }
        public decimal Quantity { get; set; }
        public ShipmentDataSource DataSource { get; set; }
    }
}
