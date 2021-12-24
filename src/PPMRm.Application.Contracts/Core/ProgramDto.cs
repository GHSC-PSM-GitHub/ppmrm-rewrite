using System;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Core
{
    public class ProgramDto : AuditedEntityDto<Programs>
    {
        public string Name { get; set; }
    }
}
