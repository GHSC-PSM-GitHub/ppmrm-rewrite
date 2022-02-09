using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Orders
{
    public interface IOrderAppService : IReadOnlyAppService<OrderDto,
                                                            OrderDto,
                                                            string,
                                                            GetOrdersDto>
    {
        Task<(List<Core.CountryDto>, List<Core.PeriodDto>)> GetFiltersAsync();
        
    }
}
