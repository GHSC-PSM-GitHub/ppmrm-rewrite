using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public class ProgramAppService : CrudAppService<
            Program,
            ProgramDto,
            string, //Primary key 
            PagedAndSortedResultRequestDto,
            ProgramDto>,
        IProgramAppService
    {
        public ProgramAppService(IRepository<Program, string> repository) : base(repository)
        {
        }
    }
}
