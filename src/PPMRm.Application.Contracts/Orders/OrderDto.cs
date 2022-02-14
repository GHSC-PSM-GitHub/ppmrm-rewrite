using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Orders
{
    public class OrderDto : AuditedEntityDto<string>
    {
        public string OrderNumber { get; set; }

        public string CountryId { get; set; }

        public string EnterpriseCode { get; set; }
        public string ParentRONumber { get; set; }
        public string RONumber { get; set; }
        public string PODOIONumber { get; set; }

        public DateTime? ParentOrderEntryDate { get; set; }
        public DateTime? PSMSourceApprovalDate { get; set; }
        public DateTime? POReleasedForFulfillmentDate { get; set; }
        public DateTime? QAInitiatedDate { get; set; }
        public DateTime? QACompletedDate { get; set; }
        public DateTime? ActualShipDate { get; set; }


        public DateTime? RequestedDeliveryDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? RevisedAgreedDeliveryDate { get; set; }

        public DateTime? LatestEstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }


        public DateTime? DisplayDate { get; set; }
        public string DeliveryDateType { get; set; }

        public string OrderType { get; set; }
        public string StatusSequence { get; set; }
        public string ExternalStatusStageSequence { get; set; }
        public List<OrderLineDto> Lines { get; set; } = new();

        public List<OrderLineDto> Shipments => Lines.Where(l => l.Item != null).OrderBy(l => l.LineNumber).ToList();

        public int NumberOfLines => Shipments.Count;
        public string LineTotal => $"{NumberOfLines} ({Lines.Count})";

    }

    public class OrderLineDto
    {
        public int LineNumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public string ProductId { get; set; }
        public Items.ItemDto Item { get; set; }
        public string ItemName => Item?.Name ?? "N/A";
        public string UOM { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BaseUnitMultiplier => Item?.BaseUnitMultiplier ?? 1;
        public decimal TotalQuantity => OrderedQuantity * BaseUnitMultiplier;
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? RevisedAgreedDeliveryDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }
        public string RDD => RequestedDeliveryDate?.ToString("yyyy-MM-dd") ?? string.Empty;
        public string RaDD => RevisedAgreedDeliveryDate?.ToString("yyyy-MM-dd") ?? string.Empty;
        public string EDD => EstimatedDeliveryDate?.ToString("yyyy-MM-dd") ?? string.Empty;
        public string AcDD => ActualDeliveryDate?.ToString("yyyy-MM-dd") ?? string.Empty;
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
