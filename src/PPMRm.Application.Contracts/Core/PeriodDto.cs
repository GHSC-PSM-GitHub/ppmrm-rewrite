using System;
using System.Globalization;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class PeriodDto : AuditedEntityDto<int>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Name => $"{StartDate:MMMM} {Year}";
        public string ShortName => $"{StartDate:MMM} {Year}";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Quarter Quarter { get; set; }
    }

    public class GetPeriodListDto : EntityDto
    {
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? StartMonth { get; set; }
        public int? EndMonth { get; set; }
    }
}
