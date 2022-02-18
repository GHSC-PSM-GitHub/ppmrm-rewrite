using PPMRm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.PeriodReports
{
    public class ProductStock : AuditedEntity
    {
        public ProductStock()
        {

        }

        public ProductStock(string periodReportId, Programs program, string productId)
        {
            PeriodReportId = periodReportId;
            ProgramId = (int)program;
            ProductId = productId;
        }

        public string PeriodReportId { get; set; }
        public int ProgramId { get; set; }
        public string ProductId { get; set; }
        public decimal SOH { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public decimal? AMC { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
        public override object[] GetKeys()
        {
            return new object[] { PeriodReportId, ProgramId, ProductId };
        }
    }
}
