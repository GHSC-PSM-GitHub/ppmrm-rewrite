using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.ProgramReports
{
    public class ProductStock : AuditedEntity
    {
        public ProductStock()
        {

        }

        public ProductStock(Guid programReportId, string productId)
        {
            ProgramReportId = programReportId;
            ProductId = productId;
        }

        public Guid ProgramReportId { get; set; }
        public string ProductId { get; set; }
        public decimal SOH { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public decimal? AMC { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
        public override object[] GetKeys()
        {
            return new object[] { ProgramReportId, ProductId };
        }
    }
}
