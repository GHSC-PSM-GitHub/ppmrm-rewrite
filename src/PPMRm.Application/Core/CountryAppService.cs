using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using System.Linq;

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
        IRepository<IdentityUser, Guid> UserRepository { get; }
        ICountryRepository CountryRepository => Repository as ICountryRepository;
        public CountryAppService(ICountryRepository repository, IRepository<IdentityUser, Guid> userRepository) : base(repository)
        {
            UserRepository = userRepository;
        }

        [Authorize]
        async public Task<PagedResultDto<CountryDto>> GetUserCountryListAsync(PagedAndSortedResultRequestDto input)
        {
            var countries = (await CountryRepository.GetUserCountriesAsync());
            var results = ObjectMapper.Map<List<Country>, List<CountryDto>>(countries.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return new PagedResultDto<CountryDto>(countries.Count, results);
        }
    }
}
