using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public class PeriodReportDetailDto : PeriodReportDto
    {
        public List<ProgramPeriodDto> Programs { get; set; } = new();
    }
}
