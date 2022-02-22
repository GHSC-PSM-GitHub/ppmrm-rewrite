using PPMRm.ARTMIS.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.ARTMIS
{
    public abstract record OrderLineEvent
    {
        public string FileName { get; set; }
        public long FileTimestamp { get; set; }
        public long EventTimestamp { get; set; }
        public int EventLineNumber { get; set; }
        public int PeriodId { get; set; }
        public string CountryId { get; set; }
        public string OrderLineId { get; set; }
        public string EnterpriseCode { get; set; }
        public string RecipientCountry { get; set; }
        public string ConsigneeCompanyName { get; set; }
        public string ExternalStatusStage { get; set; }
        public string ParentRONumber { get; set; }
        public string RONumber { get; set; }
        public int ROPrimeLineNumber { get; set; }
        public string PODOIONumber { get; set; }
        public string OrderNumber { get; set; }
        public int OrderLineNumber { get; set; }
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public string UOM { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
        public decimal OrderedQuantity { get; set; }

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
        public static OrderLineEvent Create(OrderEto o)
        {
            switch (o.ChangeIndicator)
            {
                case ChangeIndicator.Insert:
                    return new OrderLineInserted
                    {
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        ActualShipDate = o.ActualShipDate,
                        ChangeIndicator = o.ChangeIndicator,
                        ConsigneeCompanyName = o.ConsigneeCompanyName,
                        EnterpriseCode = o.EnterpriseCode,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ExternalStatusStage = o.ExternalStatusStage,
                        ExternalStatusStageSequence = o.ExternalStatusStageSequence,
                        ItemId = o.ItemId,
                        ProductId = o.ProductId,
                        LatestEstimatedDeliveryDate = o.LatestEstimatedDeliveryDate,
                        LineTotal = o.LineTotal,
                        OrderedQuantity = o.OrderedQuantity,
                        OrderLineNumber = o.OrderLineNumber,
                        OrderNumber = o.OrderNumber,
                        OrderType = o.OrderType,
                        ParentOrderEntryDate = o.ParentOrderEntryDate,
                        ParentRONumber = o.ParentRONumber,
                        PODOIONumber = o.PODOIONumber,
                        POReleasedForFulfillmentDate = o.POReleasedForFulfillmentDate,
                        PSMSourceApprovalDate = o.PSMSourceApprovalDate,
                        QACompletedDate = o.QACompletedDate,
                        QAInitiatedDate = o.QAInitiatedDate,
                        RecipientCountry = o.RecipientCountry,
                        RequestedDeliveryDate = o.RequestedDeliveryDate,
                        RevisedAgreedDeliveryDate = o.RevisedAgreedDeliveryDate,
                        RONumber = o.RONumber,
                        ROPrimeLineNumber = o.ROPrimeLineNumber,
                        StatusSequence = o.StatusSequence,
                        UnitPrice = o.UnitPrice,
                        UOM = o.UOM,
                        CountryId = o.CountryId,
                        PeriodId = o.PeriodId,
                        FileName = o.FileName,
                        EventTimestamp = o.EventTimestamp,
                        FileTimestamp = o.FileTimestamp,
                        OrderLineId = o.OrderLineId,
                        EventLineNumber = o.LineNumber
                    };
                case ChangeIndicator.Delete:
                    return new OrderLineDeleted
                    {
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        ActualShipDate = o.ActualShipDate,
                        ChangeIndicator = o.ChangeIndicator,
                        ConsigneeCompanyName = o.ConsigneeCompanyName,
                        EnterpriseCode = o.EnterpriseCode,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ExternalStatusStage = o.ExternalStatusStage,
                        ExternalStatusStageSequence = o.ExternalStatusStageSequence,
                        ItemId = o.ItemId,
                        ProductId = o.ProductId,
                        LatestEstimatedDeliveryDate = o.LatestEstimatedDeliveryDate,
                        LineTotal = o.LineTotal,
                        OrderedQuantity = o.OrderedQuantity,
                        OrderLineNumber = o.OrderLineNumber,
                        OrderNumber = o.OrderNumber,
                        OrderType = o.OrderType,
                        ParentOrderEntryDate = o.ParentOrderEntryDate,
                        ParentRONumber = o.ParentRONumber,
                        PODOIONumber = o.PODOIONumber,
                        POReleasedForFulfillmentDate = o.POReleasedForFulfillmentDate,
                        PSMSourceApprovalDate = o.PSMSourceApprovalDate,
                        QACompletedDate = o.QACompletedDate,
                        QAInitiatedDate = o.QAInitiatedDate,
                        RecipientCountry = o.RecipientCountry,
                        RequestedDeliveryDate = o.RequestedDeliveryDate,
                        RevisedAgreedDeliveryDate = o.RevisedAgreedDeliveryDate,
                        RONumber = o.RONumber,
                        ROPrimeLineNumber = o.ROPrimeLineNumber,
                        StatusSequence = o.StatusSequence,
                        UnitPrice = o.UnitPrice,
                        UOM = o.UOM,
                        CountryId = o.CountryId,
                        PeriodId = o.PeriodId,
                        FileName = o.FileName,
                        EventTimestamp = o.EventTimestamp,
                        FileTimestamp = o.FileTimestamp,
                        OrderLineId = o.OrderLineId,
                        EventLineNumber = o.LineNumber
                    };
                case ChangeIndicator.Update:
                    return new OrderLineUpdated
                    {
                        ActualDeliveryDate = o.ActualDeliveryDate,
                        ActualShipDate = o.ActualShipDate,
                        ChangeIndicator = o.ChangeIndicator,
                        ConsigneeCompanyName = o.ConsigneeCompanyName,
                        EnterpriseCode = o.EnterpriseCode,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        ExternalStatusStage = o.ExternalStatusStage,
                        ExternalStatusStageSequence = o.ExternalStatusStageSequence,
                        ItemId = o.ItemId,
                        ProductId = o.ProductId,
                        LatestEstimatedDeliveryDate = o.LatestEstimatedDeliveryDate,
                        LineTotal = o.LineTotal,
                        OrderedQuantity = o.OrderedQuantity,
                        OrderLineNumber = o.OrderLineNumber,
                        OrderNumber = o.OrderNumber,
                        OrderType = o.OrderType,
                        ParentOrderEntryDate = o.ParentOrderEntryDate,
                        ParentRONumber = o.ParentRONumber,
                        PODOIONumber = o.PODOIONumber,
                        POReleasedForFulfillmentDate = o.POReleasedForFulfillmentDate,
                        PSMSourceApprovalDate = o.PSMSourceApprovalDate,
                        QACompletedDate = o.QACompletedDate,
                        QAInitiatedDate = o.QAInitiatedDate,
                        RecipientCountry = o.RecipientCountry,
                        RequestedDeliveryDate = o.RequestedDeliveryDate,
                        RevisedAgreedDeliveryDate = o.RevisedAgreedDeliveryDate,
                        RONumber = o.RONumber,
                        ROPrimeLineNumber = o.ROPrimeLineNumber,
                        StatusSequence = o.StatusSequence,
                        UnitPrice = o.UnitPrice,
                        UOM = o.UOM,
                        CountryId = o.CountryId,
                        PeriodId = o.PeriodId,
                        FileName = o.FileName,
                        EventTimestamp = o.EventTimestamp,
                        FileTimestamp = o.FileTimestamp,
                        OrderLineId = o.OrderLineId,
                        EventLineNumber = o.LineNumber
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(o.ChangeIndicator));
            }

        }


    }

    public record OrderLineInserted : OrderLineEvent
    {
    }

    public record OrderLineUpdated : OrderLineEvent
    {
    }

    public record OrderLineDeleted : OrderLineEvent
    {

    }
}
