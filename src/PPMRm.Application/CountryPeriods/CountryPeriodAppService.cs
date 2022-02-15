using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.CountryPeriods
{
    public class CountryPeriodAppService : ApplicationService, ICountryPeriodAppService
    {
        public Task<CountryPeriodDto> CreateAsync(CountryPeriodDto input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CountryPeriodDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        async public Task<PagedResultDto<CountryPeriodDto>> GetCountryPeriodsAsync(GetCountryPeriodListDto input)
        {
            return await GetListAsync(input);
        }

        public Task<PagedResultDto<CountryPeriodDto>> GetListAsync(GetCountryPeriodListDto input)
        {
            throw new NotImplementedException();
        }

        public Task<CountryPeriodDto> UpdateAsync(Guid id, CountryPeriodDto input)
        {
            throw new NotImplementedException();
        }
    }
}
