using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public class CreateUpdateShipmentDto
    {
        public Guid? Id { get; set; }
        public string ProductId { get; set; }
        public Supplier Supplier { get; set; }
        public decimal Quantity { get; set; }
        public bool? IsTBD { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
        public ShipmentDataSource DataSource { get; set; }

    }
}
