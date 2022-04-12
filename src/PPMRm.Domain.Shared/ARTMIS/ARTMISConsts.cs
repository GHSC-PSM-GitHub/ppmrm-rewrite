using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.ARTMIS
{
    public class ARTMISConsts
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public const int TimestampLength = 14;
        public const int ProductIdLength = 12;
        public const int PeriodIdLength = 6;

        public const string ChangeIndicator = "CHANGE_IND";
        public const string EnterpriseCode = "ENTERPRISE_CODE";

        public static class ItemHeaders
        {
            public const string SupplierName = "ITEM_SUPPLIER_NAME";
            public const string ManufacturerName = "ITEM_MANUFACTURER_NAME";
            public const string ManufacturerGTIN = "ITEM_MANUFACTURER_GTIN_UPC";
            public const string TracerCategory = "ITEM_TRACER_CAT";
            public const string ProductId = "PRODUCT_ID";
            public const string ItemId = "ITEM_ID";
            public const string ProductName = "PRODUCT_NAME";
            public const string ProductNumberOfTreatments = "PRODUCT_NUM_OF_TREATMENT";
            public const string ProductNumberOfTreatmentsUOM = "PRODUCT_TREATMENT_UOM";
            public const string UOM = "ITEM_UOM";
            public const string PackSize = "ITEM_PACK_SIZE";
            public const string BaseUnit = "ITEM_BASE_UNIT";
            public const string BaseUnitMultiplier = "ITEM_BASE_UNIT_MULT";
            public const string CatalogPrice = "WCS_CATALOG_PRICE";
            public const string CountryOfOrigin = "ITEM_COUNTRY_OF_ORIGIN";
            public const string ManufacturerLocation = "ITEM_MANUFACTURER_LOCATION";
            public const string LLINDimensions = "ITEM_LLIN_DIMENSIONS";
        }

        public static class OrderHeaders
        {
            public const string EnterpriseCode = ARTMISConsts.EnterpriseCode;
            public const string RecipientCountry = "RECIPIENT_COUNTRY";
            public const string ConsigneeCompanyName = "CONSIGNEE_COMPANY_NAME";
            public const string ExternalStatusStage = "EXTERNAL_STATUS_STAGE";
            public const string ParentRONumber = "PARENT_RO";
            public const string RONumber = "RO_NUMBER";
            public const string ROPrimeLineNumber = "RO_PRIME_LINE_NO";
            public const string PODOIONumber = "PO_DO_IO_NUMBER";
            public const string OrderNumber = "ORDER_NUMBER";
            public const string OrderLineNumber = "PRIME_LINE_NO";
            public const string ItemId = "ITEM_ID";
            public const string UOM = "UOM";
            public const string UnitPrice = "UNIT_PRICE";
            public const string LineTotal = "LINE_TOTAL";
            public const string OrderedQuantity = "ORDERED_QTY";

            public const string ParentOrderEntryDate = "PARENT_ORDER_ENTRY_DATE";
            public const string PSMSourceApprovalDate = "FINAL_PSM_SOURCE_APPROVAL_DATE";
            public const string POReleasedForFulfillmentDate = "PO_RELEASED_FOR_FULFILLMENT_DATE";
            public const string QAInitiatedDate = "QA_INITIATED_DATE";
            public const string QACompletedDate = "QA_COMPLETE_DATE";
            public const string ActualShipDate = "ACTUAL_SHIP_DATE";
            public const string RequestedDeliveryDate = "REQUESTED_DELIVERY_DATE";
            public const string EstimatedDeliveryDate = "ESTIMATED_DELIVERY_DATE";
            public const string RevisedAgreedDeliveryDate = "REVISED_AGREED_DELIVERY_DATE";
            public const string LatestEstimatedDeliveryDate = "LATEST_ESTIMATED_DELIVERY_DATE";
            public const string ActualDeliveryDate = "ACTUAL_DELIVERY_DATE";
            public const string OrderType = "ORDER_TYPE_IND";
            public const string StatusSequence = "STATUS_SEQ";
            public const string ExternalStatusStageSequence = "EXTERNAL_STATUS_STAGE_SEQ";
            public const string ChangeIndicator = ARTMISConsts.ChangeIndicator;
        }

        public static class ShipmentHeaders
        {
            public const string EnterpriseCode = ARTMISConsts.EnterpriseCode;
            public const string OrderNumber = "ORDER_NO";
            public const string OrderLineNumber = "PRIME_LINE_NO";
            public const string ShipmentNumber = "KN_SHIPMENT_NUMBER";
            public const string ItemId = "ITEM_SKU_CODE";
            public const string BatchNumber = "BATCH_NO";
            public const string ContainerNumber = "CONTAINER_NO";
            public const string ActualQuantity = "ACTUAL_QUANTITY";

            public const string PickupDate = "PICK_UP_DATE";
            public const string ActualShipmentDate = "ACTUAL_SHIPMENT_DATE";
            public const string ShipmentArrivalDate = "SHIPMENT_ARRIVAL_DATE";
            public const string TotalWeight = "TOTAL_WEIGHT";
            public const string TotalVolume = "TOTAL_VOLUME";
            public const string PortOfDestination = "PORT_OF_DESTINATION";
            public const string PortOfOrigin = "PORT_OF_ORIGIN";
            public const string ChangeIndicator = ARTMISConsts.ChangeIndicator;
        }

        public static class ChangeIndicatorConsts
        {
            public const int Insert = 1;
            public const int Delete = 2;
            public const int Update = 3;
        }

        public static class OrderDeliveryDateTypes
        {
            public const string RequestedDeliveryDate = "RDD";
            public const string EstimatedDeliveryDate = "EDD";
            public const string LatestEstimatedDeliveryDate = "LeDD";
            public const string RevisedAgreedDeliveryDate = "RaDD";
            public const string ActualDeliveryDate = "AcDD";
        }

        public static readonly IReadOnlyDictionary<string, string> PPMRmProductMappings = new Dictionary<string, string>()
        {
            {"100076AAK08VP", "AL6x1-20" },
            {"100076AAA08WP", "AL6x1-20"},
            {"100076AAK08WP", "AL6x1-20"},
            {"100077AAA08VP", "AL6x1-80"},
            {"100076AAA08XP", "AL6x2-20"},
            {"100076AAK09DP", "AL6x2-20"},
            {"100076AAK08XP", "AL6x2-20"},
            {"100076AAA08YP", "AL6x3-20"},
            {"100076AAA08ZP", "AL6x4-20"},
            {"103199FPA05PP", "AS-INJ-30"},
            {"100097FPA05PP", "AS-INJ-60"},
            {"103347FPA05PP", "AS-INJ-60"},
            {"101206HLA0EYP", "AS-SUP-100"},
            {"101206HLA06AP", "AS-SUP-100"},
            {"100078HLA06AP", "AS-SUP-200"},
            {"100079HLA06AP", "AS-SUP-50"},
            {"100073AAA08UP", "ASAQ3-100"},
            {"100074AAA08UP", "ASAQ3-25"},
            {"100075AAA08UP", "ASAQ3-50"},
            {"100073AAA09AP", "ASAQ6-100"},
            {"100091AAA08AP", "PQ-15"},
            {"100092AAA08AP", "PQ-7_5"},
            {"100092AAA02IP", "PQ-7_5"},
            {"100068XYB08DP", "RDT"},
            {"100062XXC08DP", "RDT"},
            {"100066XXC08DP", "RDT"},
            {"100062XXC0GJP", "RDT"},
            {"100068XXC08TP", "RDT"},
            {"106131XXC08DP", "RDT"},
            {"100066XXC0GJP", "RDT"},
            {"105284XXC08DP", "RDT"},
            {"100068XXC08DP", "RDT"},
            {"100068XXC0GJP", "RDT"},
            {"100064XXC08DP", "RDT"},
            {"100063XXC08DP", "RDT"},
            {"100066XXC08EP", "RDT"},
            {"100041AAA0JSP", "SP"},
            {"100041AAA0G9P", "SP"},
            {"100041AAA08AP", "SP"},
            {"100041AAA07SP", "SP"},
            {"100041AAA07NP", "SP"},

        };
    }
}
