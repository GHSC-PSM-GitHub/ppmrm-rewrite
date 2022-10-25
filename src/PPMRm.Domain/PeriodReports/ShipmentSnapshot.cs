using PPMRm.ARTMIS.OrderLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace PPMRm.PeriodReports
{
    public class ShipmentSnapshot
    {
        private ShipmentSnapshot(string id)
        {
            Id = Check.NotNullOrEmpty(id, nameof(id), 10, 10);
            Shipments = new List<PMIShipment>();
        }

        public ShipmentSnapshot(string countryId, int periodId) : this($"{countryId}-{periodId}")
        {
            CountryId = Check.NotNullOrWhiteSpace(countryId, nameof(countryId));
            PeriodId = periodId;
        }
        /// <summary>
        /// PeriodReportId ({CountryId}-{PeriodId})
        /// </summary>
        public string Id { get; set; }
        public string CountryId { get; set; }
        public int PeriodId { get; set; }
        public List<PMIShipment> Shipments { get; set; }

    }

    public class PMIShipment
    {
        public string OrderLineId { get; set; }
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal BaseUnitMultiplier { get; set; }
        public decimal TotalQuantity => Quantity * BaseUnitMultiplier;
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? RevisedAgreedDeliveryDate { get; set; }
        public DateTime? LatestEstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
    }
}
