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

        async public Task<UpdateCountryDto> GetDetailsAsync(string id)
        {
            var queryable = await CountryRepository.WithDetailsAsync(c => c.Products, c => c.Programs);
            var country = await AsyncExecuter.SingleAsync(queryable.Where(c => c.Id == id));
            var result = ObjectMapper.Map<Country, UpdateCountryDto>(country);
            result.ProductIds = country.Products.Select(p => p.ProductId).ToList();
            result.ProgramIds = country.Programs.Select(p => p.ProgramId).ToList();
            return result;
        }

        [Authorize]
        async public Task<PagedResultDto<CountryDto>> GetUserCountryListAsync(PagedAndSortedResultRequestDto input)
        {
            var countries = (await CountryRepository.GetUserCountriesAsync());
            var results = ObjectMapper.Map<List<Country>, List<CountryDto>>(countries.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return new PagedResultDto<CountryDto>(countries.Count, results);
        }

        public async Task UpdateAsync(string id, UpdateCountryDto countryDto)
        {
            var queryable = await CountryRepository.WithDetailsAsync(c => c.Products, c => c.Programs);
            var country = await AsyncExecuter.SingleAsync(queryable.Where(c => c.Id == id));
            country.UpdateMinMax(countryDto.MinStock, countryDto.MaxStock);
            country.UpdateProducts(countryDto.ProductIds ?? new List<string>());
            await CountryRepository.UpdateAsync(country);
            //country.UpdateMinMax();
            
        }
    }
}
