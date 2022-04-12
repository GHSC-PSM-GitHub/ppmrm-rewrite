using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.PeriodReports
{
    public class GetPeriodReportDto : PagedResultRequestDto
    {
        public List<string> Countries { get; set; } = new();
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}
