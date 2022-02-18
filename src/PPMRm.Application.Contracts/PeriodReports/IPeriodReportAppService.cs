using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace PPMRm.PeriodReports
{
    public interface IPeriodReportAppService : IReadOnlyAppService<PeriodReportDetailDto, PeriodReportDto, string, GetPeriodReportDto>
    {
    }
}
