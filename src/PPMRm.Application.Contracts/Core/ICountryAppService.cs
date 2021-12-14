using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Core
{
    public interface ICountryAppService : ICrudAppService<
            CountryDto, 
            string, 
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CountryDto>
    {
        
    }
}
