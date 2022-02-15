using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Core
{
    public interface IPeriodAppService : IReadOnlyAppService<PeriodDto, int, GetPeriodListDto>
    {
        Task OpenAsync(int id);
    }
}
