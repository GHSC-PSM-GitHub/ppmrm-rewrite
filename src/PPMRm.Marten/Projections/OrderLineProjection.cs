using Marten.Events.Aggregation;
using Marten.Events.Projections;
using PPMRm.ARTMIS;
using PPMRm.ARTMIS.OrderLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.Projections
{
    public class OrderLineProjection : AggregateProjection<OrderLine>
    {
        public OrderLineProjection()
        {
            DeleteEvent<OrderLineDeleted>();
        }

        public void Apply(OrderLineUpdated @event, OrderLine orderLine)
        {
            orderLine.Id = @event.OrderLineId;
            orderLine.OrderNumber = @event.OrderNumber;
            orderLine.OrderLineNumber = @event.OrderLineNumber;
            orderLine.RONumber = @event.RONumber;
            orderLine.ROPrimeLineNumber = @event.OrderLineNumber;
            orderLine.CountryId = @event.CountryId;

            orderLine.ProductId = @event.ProductId;
            orderLine.ItemId = @event.ItemId;
            orderLine.PPMRmProductId = ARTMISConsts.PPMRmProductMappings.GetOrDefault(@event.ProductId);
            orderLine.OrderedQuantity = @event.OrderedQuantity;
            orderLine.UOM = @event.UOM;
            orderLine.UnitPrice = @event.UnitPrice;
            orderLine.LineTotal = @event.LineTotal;

            orderLine.ParentOrderEntryDate = @event.ParentOrderEntryDate;
            orderLine.PSMSourceApprovalDate = @event.PSMSourceApprovalDate;

            orderLine.RequestedDeliveryDate = @event.RequestedDeliveryDate;
            orderLine.EstimatedDeliveryDate = @event.EstimatedDeliveryDate;
            orderLine.LatestEstimatedDeliveryDate = @event.LatestEstimatedDeliveryDate;
            orderLine.RevisedAgreedDeliveryDate = @event.RevisedAgreedDeliveryDate;
            orderLine.ActualDeliveryDate = @event.ActualDeliveryDate;
            orderLine.ActualShipDate = @event.ActualShipDate;

            orderLine.OrderType = @event.OrderType;
            orderLine.ExternalStatusStageSequence = @event.ExternalStatusStageSequence;
            orderLine.StatusSequence = @event.StatusSequence;

            // Set PPMRM Shipment Display Date
            // Initial logic that checks LEDD and RaDD
            // orderLine.ShipmentDate = @event.ActualDeliveryDate ?? @event.LatestEstimatedDeliveryDate ?? @event.RevisedAgreedDeliveryDate ?? @event.EstimatedDeliveryDate ?? @event.RequestedDeliveryDate;
            //orderLine.ShipmentDateType = @event.ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
            //                    @event.LatestEstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
            //                    @event.RevisedAgreedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
            //                    @event.EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
            //                    @event.RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null;
            orderLine.ShipmentDate = @event.ActualDeliveryDate ?? @event.EstimatedDeliveryDate ?? @event.RequestedDeliveryDate;
            orderLine.ShipmentDateType = @event.ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
                                @event.EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null;
        }

        public OrderLine Create(OrderLineInserted @event)
        {
            var orderLine = new OrderLine();
            orderLine.Id = @event.OrderLineId;
            orderLine.OrderNumber = @event.OrderNumber;
            orderLine.OrderLineNumber = @event.OrderLineNumber;
            orderLine.RONumber = @event.RONumber;
            orderLine.ROPrimeLineNumber = @event.OrderLineNumber;
            orderLine.CountryId = @event.CountryId;

            orderLine.ProductId = @event.ProductId;
            orderLine.ItemId = @event.ItemId;
            orderLine.PPMRmProductId = ARTMISConsts.PPMRmProductMappings.GetOrDefault(@event.ProductId);
            orderLine.OrderedQuantity = @event.OrderedQuantity;
            orderLine.UOM = @event.UOM;
            orderLine.UnitPrice = @event.UnitPrice;
            orderLine.LineTotal = @event.LineTotal;

            orderLine.ParentOrderEntryDate = @event.ParentOrderEntryDate;
            orderLine.PSMSourceApprovalDate = @event.PSMSourceApprovalDate;

            orderLine.RequestedDeliveryDate = @event.RequestedDeliveryDate;
            orderLine.EstimatedDeliveryDate = @event.EstimatedDeliveryDate;
            orderLine.LatestEstimatedDeliveryDate = @event.LatestEstimatedDeliveryDate;
            orderLine.RevisedAgreedDeliveryDate = @event.RevisedAgreedDeliveryDate;
            orderLine.ActualDeliveryDate = @event.ActualDeliveryDate;
            orderLine.ActualShipDate = @event.ActualShipDate;

            orderLine.OrderType = @event.OrderType;
            orderLine.ExternalStatusStageSequence = @event.ExternalStatusStageSequence;
            orderLine.StatusSequence = @event.StatusSequence;

            // Set PPMRM Shipment Display Date
            orderLine.ShipmentDate = @event.ActualDeliveryDate ?? @event.LatestEstimatedDeliveryDate ?? @event.RevisedAgreedDeliveryDate ?? @event.EstimatedDeliveryDate ?? @event.RequestedDeliveryDate;
            orderLine.ShipmentDateType = @event.ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
                                @event.LatestEstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.RevisedAgreedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                @event.RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null;
            return orderLine;
        }

    }
}
