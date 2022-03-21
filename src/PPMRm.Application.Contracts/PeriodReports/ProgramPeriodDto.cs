using PPMRm.Core;
using PPMRm.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public class ProgramPeriodDto
    {
        public string PeriodReportId { get; set; }
        public int ProgramId { get; set; }
        public ProgramDto Program { get; set; }
        public List<ProgramProductDto> Products { get; set; }
    }

    public class ProgramProductDto
    {
        public PeriodReportStatus ReportStatus { get; set; }
        public ProgramDto Program { get; set; }
        public ProductDto Product { get; set; }
        public decimal? SOH { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS => SOH != null && AMC != null && AMC.Value > 0 ? SOH / AMC : null;
        public MOSStatus? MOSStatus => !MOS.HasValue ? null :
                                        MOS == 0 ? PeriodReports.MOSStatus.StockedOut :
                                        MOS < MinStock ? PeriodReports.MOSStatus.BelowMin :
                                        MOS >= MinStock && MOS <= MaxStock ? PeriodReports.MOSStatus.MinToMax :
                                        PeriodReports.MOSStatus.OverStocked;
        public List<string> SOHLevels { get; set; }
        public SourceOfConsumption? SourceOfConsumption { get; set; }
        public string OtherSourceOfConsumption { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
        public List<ProductShipmentDto> Shipments { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }

    }

    public class ProductShipmentDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public Supplier Supplier { get; set; }
        public decimal Quantity { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS => AMC != null && AMC.Value > 0 ? Quantity / AMC : null;
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
        public ShipmentDataSource DataSource { get; set; }

    }
}
