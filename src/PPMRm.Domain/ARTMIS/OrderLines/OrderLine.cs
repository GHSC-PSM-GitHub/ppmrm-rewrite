using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.OrderLines
{
    public class OrderLine
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public int OrderLineNumber { get; set; }
        public string CountryId { get; set; }
        public string RONumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        /// <summary>
        /// ARTMIS Product ID
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// ARTMIS Item ID
        /// </summary>
        public string ItemId { get; set; }
        public string PPMRmProductId { get; set; }
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


    }
}
