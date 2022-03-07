using System;
using System.Collections.Generic;
using System.Text;

namespace PPMRm.PeriodReports
{
    public class CreateUpdateProgramProductDto
    {
        public decimal? SOH { get; set; }
        public decimal? AMC { get; set; }
        public decimal? MOS => SOH != null && AMC != null && AMC.Value > 0 ? SOH / AMC : null;
        public DateTime? DateOfSOH { get; set; }
        public string ActionRecommended { get; set; }
        public DateTime? DateActionNeededBy { get; set; }
    }
}
