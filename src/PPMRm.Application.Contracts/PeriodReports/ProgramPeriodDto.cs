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
        public ProgramDto Program { get; set; }
        public ProductDto Product { get; set; }
        public decimal? SOH { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public List<ProductShipmentDto> Shipments { get; set; }

    }

    public class ProductShipmentDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public Supplier Supplier { get; set; }
        public decimal Quantity { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS { get; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentDateType ShipmentDateType { get; set; }
        public ShipmentDataSource DataSource { get; set; }

    }
}
