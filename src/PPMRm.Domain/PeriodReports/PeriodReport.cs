using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.PeriodReports
{
    public class PeriodReport : FullAuditedAggregateRoot<string>
    {
        public PeriodReport()
        {

        }
        internal PeriodReport(string countryId, int periodId)
        {
            Check.NotNullOrWhiteSpace(countryId, nameof(countryId), 3, 3);
            Id = $"{countryId}-{periodId}";
            CountryId = countryId;
            PeriodId = periodId;
            CommoditySecurityUpdates = new CommoditySecurityUpdates();
            ReportStatus = PeriodReportStatus.Initialized;
        }

        public string CountryId { get; private set; }
        public int PeriodId { get; private set; }
        public CommoditySecurityUpdates CommoditySecurityUpdates { get; set; }

        public PeriodReportStatus? ReportStatus { get; private set; }

        public void SetCSUpdates(CommoditySecurityUpdates csUpdates)
        {
            CommoditySecurityUpdates = csUpdates ?? throw new ArgumentNullException(nameof(csUpdates));
        }

        public void Open() => ReportStatus = PeriodReportStatus.Open;
        public void Close() => ReportStatus = PeriodReportStatus.Closed;
        public void Reopen() => ReportStatus = PeriodReportStatus.Reopened;

    }
}
