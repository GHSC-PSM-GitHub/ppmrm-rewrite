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
        private PeriodReport()
        {

        }
        internal PeriodReport(string countryId, int periodId) : base($"{countryId}-{periodId}")
        {   
            CountryId = Check.NotNullOrWhiteSpace(countryId, nameof(countryId));
            PeriodId = periodId;
            CommoditySecurityUpdates = new CommoditySecurityUpdates();
            ReportStatus = PeriodReportStatus.Initialized;
            ProductShipments = new List<ProductShipment>();
            ProductStocks = new List<ProductStock>();
        }

        public string CountryId { get; private set; }
        public int PeriodId { get; private set; }
        public CommoditySecurityUpdates CommoditySecurityUpdates { get; private set; }

        public PeriodReportStatus? ReportStatus { get; private set; }

        public ICollection<ProductShipment> ProductShipments { get; private set; }
        public ICollection<ProductStock> ProductStocks { get; private set; }

        public void SetCSUpdates(CommoditySecurityUpdates csUpdates)
        {
            CommoditySecurityUpdates = Check.NotNull(csUpdates, nameof(csUpdates));
        }

        public void Open() => ReportStatus = PeriodReportStatus.Open;
        public void Close() => ReportStatus = PeriodReportStatus.Closed;
        public void Reopen() => ReportStatus = PeriodReportStatus.Reopened;

    }
}
