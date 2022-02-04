using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenEye.Aggregates;
using Volo.Abp;

namespace PPMRm.ARTMIS.Orders
{
    public class Order : Aggregate
    {

        #region Properties
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
        public List<OrderLine> Lines { get; set; } = new();
        #endregion

        #region Extended ES
        //public void Apply(OrderCreated @event)
        //{
        //    Id = @event.OrderNumber;
        //    OrderNumber = @event.OrderNumber;
        //    CountryId = @event.CountryId;
        //    EnterpriseCode = @event.EnterpriseCode;
        //    ParentRONumber = @event.ParentRONumber;
        //    RONumber = @event.RONumber;
        //    PODOIONumber = @event.PODOIONumber;
        //    Version++;
        //}

        //public void Apply(OrderLineRemoved @event)
        //{
        //    ApplyHeaders(@event);
        //    Lines.RemoveAll(l => l.LineNumber == @event.OrderLineNumber);
        //    Version++;
        //}

        //public void Apply(OrderLineUpdated @event)
        //{
        //    ApplyHeaders(@event);
        //    Lines.RemoveAll(l => l.LineNumber == @event.OrderLineNumber);
        //    Lines.Add(CreateOrderLine(@event));
        //    Version++;
        //}

        //public void Apply(OrderLineInserted @event)
        //{
        //    ApplyHeaders(@event);
        //    Lines.RemoveAll(l => l.LineNumber == @event.OrderLineNumber);
        //    Lines.Add(CreateOrderLine(@event));
        //    Version++;
        //}
        #endregion

        public void Apply(OrderEvent @event)
        {
            ApplyHeaders(@event);
            Lines.RemoveAll(l => l.LineNumber == @event.OrderLineNumber);
            if(@event.ChangeIndicator != ChangeIndicator.Delete)
            {
                Lines.Add(CreateOrderLine(@event));
            }           
            Version++;
        }
        void ApplyHeaders(OrderEvent @event)
        {
            OrderNumber = @event.OrderNumber;
            CountryId = @event.RecipientCountry;
            EnterpriseCode = @event.EnterpriseCode;
            ParentRONumber = @event.ParentRONumber;
            RONumber = @event.RONumber;
            PODOIONumber = @event.PODOIONumber;

            RequestedDeliveryDate = @event.RequestedDeliveryDate;
            EstimatedDeliveryDate = @event.EstimatedDeliveryDate;
            LatestEstimatedDeliveryDate = @event.LatestEstimatedDeliveryDate;
            RevisedAgreedDeliveryDate = @event.RevisedAgreedDeliveryDate;
            ActualDeliveryDate = @event.ActualDeliveryDate;
            ActualShipDate = @event.ActualShipDate;

            // Apply extraneous properties
            OrderType = @event.OrderType;
            StatusSequence = @event.StatusSequence;
            ExternalStatusStageSequence = @event.ExternalStatusStageSequence;

            // Apply Order Display Date
            DisplayDate = ActualDeliveryDate ?? EstimatedDeliveryDate ?? RequestedDeliveryDate;
            DeliveryDateType = ActualDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate :
                                EstimatedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.EstimatedDeliveryDate :
                                RequestedDeliveryDate != null ? ARTMISConsts.OrderDeliveryDateTypes.RequestedDeliveryDate : null;
        }

        static OrderLine CreateOrderLine(OrderEvent @event)
        {
            return new OrderLine
            {
                LineNumber = @event.OrderLineNumber,
                ROPrimeLineNumber = @event.ROPrimeLineNumber,
                ItemId = @event.ItemId,
                ProductId = @event.ProductId,
                UOM = @event.UOM,
                OrderedQuantity = @event.OrderedQuantity.GetValueOrDefault()
            };
        }
    }
}
