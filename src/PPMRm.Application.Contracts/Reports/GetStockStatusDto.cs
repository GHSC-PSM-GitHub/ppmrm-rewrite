using System.Collections.Generic;

namespace PPMRm.Reports
{
    public class GetStockStatusDto
    {
        public List<string> CountryIds { get; set; }
        public List<int> ProgramIds { get; set; }
        public List<string> ProductIds { get; set; }
        public int StartPeriodId { get; set; }
        public int EndPeriodId { get; set; }
    }
}
