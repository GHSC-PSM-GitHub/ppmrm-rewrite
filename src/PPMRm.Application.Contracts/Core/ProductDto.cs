using System;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class ProductDto : AuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string BaseUnit { get; set; }
        public decimal BaseUnitMultiplier { get; set; }
    }
}
