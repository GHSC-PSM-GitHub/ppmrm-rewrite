using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Core
{
    public interface IProgramAppService : ICrudAppService<
            ProgramDto, 
            int, 
            PagedAndSortedResultRequestDto,
            ProgramDto>
    {
    }
}
