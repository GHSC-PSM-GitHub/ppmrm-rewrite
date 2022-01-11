using System;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Orders
{
    public class OrderDto : AuditedEntityDto<string>
    {
        public string Country { get; set; }
        public string RONumber { get; set; }
        public string CurrentStatus { get; set; }
        public string RDD { get; set; }
        public string EDD { get; set; }
        public string AcDD { get; set; }

    }

    public class ShipmentDto : AuditedEntityDto<string>
    {
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseUnitMultiplier { get; set; }
        public string RDD { get; set; }
        public string EDD { get; set; }
        public string AcDD { get; set; }
    }
}
