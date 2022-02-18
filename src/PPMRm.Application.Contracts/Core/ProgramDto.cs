using System;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class ProgramDto : AuditedEntityDto<int>
    {
        public string Name { get; set; }
    }
}
