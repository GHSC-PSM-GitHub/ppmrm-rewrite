using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using PPMRm.Core;
namespace PPMRm.PeriodReports
{
    public class PeriodReportDto : EntityDto<string>
    {
        public bool IsQuarterReport => Period?.Month % 3 == 0;
        public CommoditySecurityUpdatesDto CommoditySecurityUpdates { get; set; }
        public CountryDto Country { get; set; }
        public PeriodDto Period { get; set; }
        public int ShipmentsCount { get; set; }
        public int ProductsCount { get; set; }
        public PeriodReportStatus? ReportStatus { get; set; }
    }
}
