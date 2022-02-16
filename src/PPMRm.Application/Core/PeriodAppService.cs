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
            var startYear = input.StartYear ?? 2021;
            var startMonth = input.StartMonth ?? 1;
            var endYear = input.EndYear ?? 2100;
            var endMonth = input.EndMonth ?? 12;
            return Filter(startMonth, startYear, endMonth, endYear);
            //var startPeriod = Convert.ToInt32($"{startYear}{startMonth:00}");
            //var endPeriod = Convert.ToInt32($"{endYear}{endMonth:00}");
            //return p =>
            //    p.Id >= startPeriod && p.Id <= endPeriod;
        }

        static Expression<Func<Period, bool>> Filter(int startMonth, int startYear, int endMonth, int endYear)
        {
            return p =>
                p.Year >= startYear && p.Year <= endYear &&
                (
                    (p.Year == startYear && p.Month >= startMonth) ||
                    (p.Year == endYear && p.Month <= endMonth) ||
                    (p.Year > startYear && p.Year < endYear)
                );
        }
    }
}
