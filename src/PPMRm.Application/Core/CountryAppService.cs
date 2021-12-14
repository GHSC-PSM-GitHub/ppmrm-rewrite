using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public class CountryAppService : CrudAppService<
            Country,
            CountryDto,
            string, //Primary key 
            PagedAndSortedResultRequestDto,
            CountryDto>,
        ICountryAppService
    {
        public CountryAppService(IRepository<Country, string> repository) : base(repository)
        {
        }
    }
}
