using System;
using Volo.Abp.Application.Services;

namespace PPMRm.Orders
{
    public interface IOrderAppService : IReadOnlyAppService<OrderDto, string>
    {
        
    }
}
