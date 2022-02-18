using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PPMRm.ProgramReports
{
    public class ProgramShipment : Entity<Guid>
    {
        public ProgramShipment()
        {

        }

        internal ProgramShipment(Guid id, Guid programReportId, Supplier supplier, string productId, decimal quantity, DateTime? shipmentDate, ShipmentDateType shipmentDateType, ShipmentDataSource dataSource) : base(id)
        {
            Check.NotNull(productId, nameof(productId));
            ProgramReportId = programReportId;
            Supplier = supplier;
            ProductId = productId;
            Quantity = quantity;
            ShipmentDate = shipmentDate;
            ShipmentDateType = shipmentDateType;
            DataSource = dataSource;
        }
        public Guid ProgramReportId { get; set; }
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
