using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Core
{
    public interface IProgramAppService : ICrudAppService<
            ProgramDto, 
            Programs, 
            PagedAndSortedResultRequestDto,
            ProgramDto>
    {
    }
}
