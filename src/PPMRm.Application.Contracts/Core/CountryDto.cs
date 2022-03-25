using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class CountryDto : AuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string ARTMISName { get; set; }

        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
    }

    public class UpdateCountryDto : CountryDto
    {
        public List<string> ProductIds { get; set; } = new();
        public List<int> ProgramIds { get; set; } = new();
    }
}
