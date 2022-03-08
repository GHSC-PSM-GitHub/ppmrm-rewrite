using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public class CreateUpdateProgramProductDto
    {
        public List<SOHLevel> SOHLevels { get; set; }
        public decimal SOH { get; set; }
        public decimal AMC { get; set; }
        public decimal? MOS => SOH != 0 && AMC != 0 && AMC > 0 ? SOH / AMC : null;
        public SourceOfConsumption SourceOfConsumption { get; set; }
        public DateTime? DateOfSOH { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
    }
}
