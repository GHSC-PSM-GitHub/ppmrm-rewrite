using System;
using System.Collections.Generic;
using PPMRm.Core;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Entities
{
    public class Order : AggregateBase, IHasCreationTime, IHasModificationTime
    {
        protected Order() { }
        public Order(string id, string countryId, Programs programId) : base()
        {
            Check.NotNullOrWhiteSpace(id, nameof(id));
            Check.NotNull(countryId, nameof(countryId));
            Id = id;
            var @event = new Events.OrderCreated
            {
                OrderNumber = id,
                CountryId = countryId,
                CreationTime = DateTime.UtcNow,
                ProgramId = (int)programId
            };
            Apply(@event);
            AddUncommittedEvent(@event);
            Id = id;
            CountryId = countryId;
            Lines = new List<OrderLine>();
            Snapshots = new List<(string, long, DateTime)>();
            // Set ProgramId based on Country
            ProgramId = (int)Programs.NationalMalariaProgram;
        }

        public void Apply(Events.OrderCreated @event)
        {
            OrderNumber = @event.OrderNumber;
            CountryId = @event.CountryId;
            ProgramId = @event.ProgramId;
            CreationTime = @event.CreationTime;
        }

        public void Apply(Events.OrderLineEvent @event)
        {
            // Apply Header properties
            EnterpriseCode = @event.EnterpriseCode;
            ParentRONumber = @event.ParentRONumber;
            RONumber = @event.RONumber;
            PODOIONumber = @event.PODOIONumber;

            // Apply Core Order properties
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

            //Apply OrderLine properties
            var orderLine = new OrderLine(@event.OrderLineNumber.GetValueOrDefault(),
                @event.ProductId,
                @event.ItemId,
                @event.UOM,
                @event.OrderedQuantity.GetValueOrDefault());
            Lines.RemoveAll(l => l.OrderLineNumber == orderLine.OrderLineNumber);

            if(@event.ChangeIndicator != ChangeIndicator.Delete)
            {
                Lines.Add(orderLine);
            }
            Version++;            
        }

        public void Apply(Events.OrderSnapshotCreated @event)
        {
            Snapshots.Add((@event.SnapshotId, @event.Version, @event.CreationTime));
            Version++;
        }

        public void AddSnapshot(string id)
        {
            var @event = new Events.OrderSnapshotCreated { OrderId = Id, SnapshotId = id, Version = Version, CreationTime = DateTime.UtcNow };
            Apply(@event);
            AddUncommittedEvent(@event);
        }

        #region Properties
        public List<(string, long, DateTime)> Snapshots { get; private set; }
        public List<OrderLine> Lines { get; private set; }
        public string OrderNumber { get; private set; }
        public string CountryId { get; private set; }
        public int ProgramId { get; private set; }

        public string EnterpriseCode { get; private set; }
        public string ParentRONumber { get; private set; }
        public string RONumber { get; private set; }
        public string PODOIONumber { get; private set; }

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

        public string OrderType { get; set; }
        public string StatusSequence { get; set; }
        public string ExternalStatusStageSequence { get; set; }

        public DateTime CreationTime { get; private set; }

        public DateTime? LastModificationTime { get; set; }

        #endregion
    }
}
