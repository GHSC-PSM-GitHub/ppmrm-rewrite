using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public class PeriodAppService : ReadOnlyAppService<Period, PeriodDto, int, GetPeriodListDto>, IPeriodAppService
    {
        public PeriodAppService(IReadOnlyRepository<Period, int> periodRepository) : base(periodRepository)
        {
        }

        async public override Task<PagedResultDto<PeriodDto>> GetListAsync(GetPeriodListDto input)
        {
            var yearPeriods = await Repository.GetListAsync(Filter(input));
            var result = ObjectMapper.Map<IEnumerable<Period>,List<PeriodDto>>(yearPeriods.OrderBy(p => p.Year).ThenBy(p => p.Month));
            return new PagedResultDto<PeriodDto>(result.Count, result);
        }

        public Task OpenAsync(int id)
        {
            //TODO: Move to ICountryPeriodAppService
            throw new NotImplementedException();
        }

        static Expression<Func<Period, bool>> Filter(GetPeriodListDto input)
        {
            if (input == null) return p => true;
            return p =>
                (!input.StartYear.HasValue || p.Year >= input.StartYear.Value) &&
                (!input.EndYear.HasValue || p.Year <= input.EndYear.Value) &&
                (!input.StartMonth.HasValue || p.Month >= input.StartMonth.Value) &&
                (!input.EndMonth.HasValue || p.Month <= input.EndMonth.Value);
        }
    }
}
