﻿using System;
using System.Threading.Tasks;
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
        Task<PagedResultDto<CountryDto>> GetUserCountryListAsync(PagedAndSortedResultRequestDto input);

        Task UpdateAsync(string id, UpdateCountryDto countryDto);
        Task<UpdateCountryDto> GetDetailsAsync(string id);
    }
}
