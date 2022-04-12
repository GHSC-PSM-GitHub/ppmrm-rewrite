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
        public SourceOfConsumption SourceOfConsumption { get; protected set; }
        public string OtherSourceOfConsumption { get; protected set; }
        /// <summary>
        /// Flag Enum that uses bitwise values and operators
        /// </summary>
        public SOHLevel? SOHLevels { get; protected set; }

        public List<SOHLevel> GetSOHLevelsList() => SOHLevels
            .ToString() // Convert the enum to string
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) // Converts the string to Enumerable of string
            .Select(//converts each element of the list to an enum, and makes an Enumerable out of the newly-converted items
                strenum =>
                {
                    SOHLevel outenum;
                    Enum.TryParse(strenum, out outenum);
                    return outenum;
                })
            .ToList();
        internal void Update(List<SOHLevel> sohLevels, decimal soh, DateTime? sohDate, decimal amc, SourceOfConsumption sourceOfConsumption, string actionRecommended, DateTime? actionNeededBy, string otherSourceOfConsumption = null)
        {
            var selectedSohLevels = sohLevels?.Distinct() ?? default;
            
            SOHLevels = selectedSohLevels.Any() ? selectedSohLevels.Aggregate((prev, next) => prev | next) : null;
            SOH = soh;
            DateOfSOH = sohDate;
            AMC = amc;
            ActionRecommended = actionRecommended;
            DateActionNeededBy = actionNeededBy;
            SourceOfConsumption = sourceOfConsumption;
            OtherSourceOfConsumption = sourceOfConsumption == SourceOfConsumption.Other ? otherSourceOfConsumption : null;
            //TODO: Add SoHLevels and sourceOfConsumtion
        }
        public override object[] GetKeys()
        {
            return new object[] { PeriodReportId, ProgramId, ProductId };
        }
    }
}
