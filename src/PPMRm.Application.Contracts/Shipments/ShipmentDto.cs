using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.Shipments
{
    public class ShipmentDto
    {
        public string CountryId { get; set; }
        public string OrderNumber { get; set; }
        public string RONumber { get; set; }
        public int LineNumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public Items.ItemDto Item { get; set; }
        public string UOM { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BaseUnitMultiplier => Item?.BaseUnitMultiplier ?? 1;
        public decimal TotalQuantity => OrderedQuantity * BaseUnitMultiplier;
    }
}
