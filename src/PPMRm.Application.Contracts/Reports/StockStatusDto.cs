using PPMRm.PeriodReports;
using System;
using System.Collections.Generic;

namespace PPMRm.Reports
{
    public class StockStatusDto
    {
        public Core.CountryDto Country { get; set; }
        public Core.ProgramDto Program { get; set; }
        public Products.ProductDto Product { get; set; }
        public Core.PeriodDto Period { get; set; }
        public string SOHLevels { get; set; }
        public decimal SOH { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS => AMC.HasValue && AMC > 0 ? SOH / AMC : null;
        public DateTime? DateOfSOH { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public string SourceOfConsumption { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
        public List<ShipmentSummaryDto> Shipments { get; set; }
        public string ShipmentSummary => string.Join(" ", Shipments ?? new List<ShipmentSummaryDto>());
    }

    public class ShipmentSummaryDto
    {
        public Guid Id { get; set; }
        public string Supplier { get; set; }
        public decimal Quantity { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS => AMC != null && AMC.Value > 0 ? Quantity / AMC : null;
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
        public ShipmentDataSource DataSource { get; set; }
        public override string ToString()
        {
            return $"[{Supplier}-{ShipmentDate?.ToString(PPMRm.PeriodReports.PeriodReportConsts.DataFormatting.DateFormat)}({ShipmentDateType})-{Quantity}-{MOS?.ToString("N1")}-{DataSource}]";
        }
    }
}
