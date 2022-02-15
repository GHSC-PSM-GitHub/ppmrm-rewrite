using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public class ProgramAppService : CrudAppService<
            Program,
            ProgramDto,
            Programs, //Primary key 
            PagedAndSortedResultRequestDto,
            ProgramDto>,
        IProgramAppService
    {
        public ProgramAppService(IRepository<Program, Programs> repository) : base(repository)
        {
        }
    }
}
