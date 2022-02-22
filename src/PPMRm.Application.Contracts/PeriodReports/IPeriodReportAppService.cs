using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PPMRm.PeriodReports
{
    public interface IPeriodReportAppService : IReadOnlyAppService<PeriodReportDetailDto, PeriodReportDto, string, GetPeriodReportDto>
    {
        //Task<PeriodReportDetailDto> GetAsync(string countryId, int period);
    }
}
