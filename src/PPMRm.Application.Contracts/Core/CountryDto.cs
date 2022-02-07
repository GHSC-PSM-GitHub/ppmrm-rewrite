using System;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class CountryDto : AuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string ARTMISName { get; set; }
    }
}
