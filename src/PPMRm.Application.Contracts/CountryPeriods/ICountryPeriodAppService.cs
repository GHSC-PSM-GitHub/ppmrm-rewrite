using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.CountryPeriods
{
    public interface ICountryPeriodAppService : ICrudAppService<CountryPeriodDto, Guid, GetCountryPeriodListDto>
    {
        Task<PagedResultDto<CountryPeriodDto>> GetCountryPeriodsAsync(GetCountryPeriodListDto input);
    }
}
