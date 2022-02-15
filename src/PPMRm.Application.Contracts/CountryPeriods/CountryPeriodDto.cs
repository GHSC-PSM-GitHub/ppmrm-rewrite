using System;
using PPMRm.Core;
using Volo.Abp.Application.Dtos;

namespace PPMRm.CountryPeriods
{
    public class CountryPeriodDto : EntityDto<Guid>
    {
        public CountryDto Country { get; set; }
        public PeriodDto Period { get; set; }
        public CommoditySecurityUpdatesDto CommoditySecurityUpdates { get; set; }
        public CountryPeriodStatus Status { get; set; }
    }

    public class GetCountryPeriodListDto : IPagedAndSortedResultRequest
    {
        public string CountryId { get; set; }
        public string[] CountryIds { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? StartMonth { get; set; }
        public int? EndMonth { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Sorting { get; set; }
        //public int[] ProgramIds { get; set; }
    }
}
