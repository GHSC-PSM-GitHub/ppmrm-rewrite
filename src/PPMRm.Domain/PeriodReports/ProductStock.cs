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
        private ProductStock()
        {

        }

        internal ProductStock(string periodReportId, Programs program, string productId) : this(periodReportId, (int)program, productId)
        {
        }

        internal ProductStock(string periodReportId, int programId, string productId)
        {
            PeriodReportId = periodReportId;
            ProgramId = programId;
            ProductId = productId;
        }

        public string PeriodReportId { get; protected set; }
        public int ProgramId { get; protected set; }
        public string ProductId { get; protected set; }
        public decimal SOH { get; protected set; }
        public DateTime? DateOfSOH { get; protected set; }
        public decimal? AMC { get; protected set; }
        public string ActionRecommended { get; protected set; }
        public DateTime? DateActionNeededBy { get; protected set; }

        internal void Update(List<SOHLevel> sohLevels, decimal soh, DateTime? sohDate, decimal amc, SourceOfConsumption sourceOfConsumption, string actionRecommended, DateTime? actionNeededBy, string otherSourceOfConsumption = null)
        {
            SOH = soh;
            DateOfSOH = sohDate;
            AMC = amc;
            ActionRecommended = actionRecommended;
            DateActionNeededBy = actionNeededBy;
            //TODO: Add SoHLevels and sourceOfConsumtion
        }
        public override object[] GetKeys()
        {
            return new object[] { PeriodReportId, ProgramId, ProductId };
        }
    }
}
