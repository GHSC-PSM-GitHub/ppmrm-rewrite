using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateType = PPMRm.PeriodReports.ShipmentDateType;
using Volo.Abp;

namespace PPMRm.ARTMIS.PeriodShipments
{
    public class PeriodShipment
    {
        public PeriodShipment() { }
        public string Id { get; set; }
        public string CountryId { get; set; }
        public int PeriodId { get; set; }

        public List<ShipmentLine> Shipments { get; set; } = new();

        public void Apply(OrderLineInserted @event)
        {
            Shipments.Add(ShipmentLine.Create(@event));
        }

        public void Apply(OrderLineDeleted @event) => Shipments.RemoveAll(s => s.Id == @event.OrderLineId);

        public void Apply(OrderLineUpdated @event)
        {
            Shipments.RemoveAll(s => s.Id == @event.OrderLineId);
            Shipments.Add(ShipmentLine.Create(@event));
        }
    }

    public class ShipmentLine
    {
        /// <summary>
        /// ARTMIS Order Line Id (OrderNumber - Line Number)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// ARTMIS Product Id
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// PPMRm Product ID
        /// </summary>
        public string PPMRmProductId { get; set; }
        public decimal OrderedQuantity { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string ShipmentDateType { get; set; }
        public DateType GetShipmentDateType() => ShipmentDateType == ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate ? DateType.AcDD :
                   ShipmentDateType == ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate ? DateType.EDD :
                    DateType.RDD;

        public static ShipmentLine Create(OrderLineEvent @event)
        {
            return new ShipmentLine
            {
                Id = @event.OrderLineId,
                ProductId = @event.ProductId,
                PPMRmProductId = ARTMISConsts.PPMRmProductMappings.GetOrDefault(@event.ProductId),
                OrderedQuantity = @event.OrderedQuantity,
                // Initial logic that checks LEDD and RaDD
                ShipmentDate = @event.ActualDeliveryDate ?? @event.LatestEstimatedDeliveryDate ?? @event.RevisedAgreedDeliveryDate ?? @event.EstimatedDeliveryDate ?? @event.RequestedDeliveryDate,
                ShipmentDateType = @event.ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
                                @event.LatestEstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.RevisedAgreedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null
                //ShipmentDate = @event.ActualDeliveryDate ?? @event.EstimatedDeliveryDate ?? @event.RequestedDeliveryDate,
                //ShipmentDateType = @event.ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
                //                @event.EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                //                @event.RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null
            };
        }

    }
}
