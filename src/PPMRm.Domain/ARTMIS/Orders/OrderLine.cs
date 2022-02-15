using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS.Orders
{
    public class OrderLine
    {
        public int LineNumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string UOM { get; set; }
        public decimal OrderedQuantity { get; set; }
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
    }
}
