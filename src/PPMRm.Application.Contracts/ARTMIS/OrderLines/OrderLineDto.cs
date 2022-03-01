using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.ARTMIS.OrderLines
{
    public class OrderLineDto : EntityDto<string>
    {
        public string OrderNumber { get; set; }
        public int OrderLineNumber { get; set; }
        public string CountryId { get; set; }
        public Core.CountryDto Country { get; set; }
        public string RONumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public Items.ItemDto Item { get; set; }
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public string PPMRmProductId { get; set; }
        public Products.ProductDto Product { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal OrderedQuantity { get; set; }

        public DateTime? ParentOrderEntryDate { get; set; }
        public DateTime? PSMSourceApprovalDate { get; set; }
        public DateTime? POReleasedForFulfillmentDate { get; set; }
        public DateTime? ActualShipDate { get; set; }


        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? RevisedAgreedDeliveryDate { get; set; }
        public DateTime? LatestEstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public string OrderType { get; set; }
        public string StatusSequence { get; set; }
        public string ExternalStatusStageSequence { get; set; }

        public DateTime? ShipmentDate { get; set; }
        public string ShipmentDateType { get; set; }
        public decimal? TotalQuantity => OrderedQuantity * (Item?.BaseUnitMultiplier ?? 1);
    }
}
