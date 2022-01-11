using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPMRm.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Orders
{
    public class OrderAppService : ReadOnlyAppService<Order, OrderDto, string>, IOrderAppService
    {
        public OrderAppService(IRepository<Order, string> repository) : base(repository)
        {
        }

        async public override Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await Task.CompletedTask;
            var orders = new List<OrderDto>
            {

            };
            return new PagedResultDto<OrderDto>(0, orders.AsReadOnly());
        }

        async public Task<PagedResultDto<ShipmentDto>> GetShipmentsListAsync(PagedAndSortedResultRequestDto input)
        {
            await Task.CompletedTask;
            var shipments = new List<ShipmentDto>();
            return new PagedResultDto<ShipmentDto>(0, shipments.AsReadOnly());
        }
    }
}
