using System;
using PPMRm.Core;

namespace PPMRm.Entities.Events
{
    public record OrderCreated
    {
        public string OrderNumber { get; set; }
        public string CountryId { get; set; }
        public int ProgramId { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public record OrderLineEvent
    {
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public string EnterpriseCode { get; set; }
        public string RecipientCountry { get; set; }
        public string ConsigneeCompanyName { get; set; }
        public string ExternalStatusStage { get; set; }
        public string ParentRONumber { get; set; }
        public string RONumber { get; set; }
        public int? ROPrimeLineNumber { get; set; }
        public string PODOIONumber { get; set; }
        public string OrderNumber { get; set; }
        public int? OrderLineNumber { get; set; }

        public string ProductId { get; set; }
        public string ItemId { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal? OrderedQuantity { get; set; }

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
        public string OrderType { get; set; }
        public string StatusSequence { get; set; }
        public string ExternalStatusStageSequence { get; set; }
        public ChangeIndicator ChangeIndicator { get; set; }
    }

    public record OrderSnapshotCreated
    {
        public string OrderId { get; set; }
        public string SnapshotId { get; set; }
        public long Version { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
