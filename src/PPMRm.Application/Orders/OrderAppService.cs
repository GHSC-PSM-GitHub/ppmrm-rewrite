using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Marten.Pagination;
using PPMRm.Core;
using PPMRm.Entities;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        IDocumentStore DocumentStore { get; }
        ICountryAppService CountryAppService { get; }
        IPeriodAppService PeriodAppService { get; }
        public OrderAppService(IDocumentStore documentStore, IPeriodAppService periodAppService, ICountryAppService countryAppService)
        {
            DocumentStore = documentStore;
            CountryAppService = countryAppService;
            PeriodAppService = periodAppService;
        }

        public async Task<OrderDto> GetAsync(string id)
        {
            using var session = DocumentStore.LightweightSession();
            var order = await session.Query<ARTMIS.Orders.Order>().Where(o => o.Lines.Any() && (o.OrderNumber == id || o.RONumber == id)).FirstOrDefaultAsync();
            if (order == null) return null;
            var result = ObjectMapper.Map<ARTMIS.Orders.Order, OrderDto>(order);
            var products = await session.Query<Items.Item>().ToListAsync();
            foreach (var l in result.Lines)
            {
                l.Item = ObjectMapper.Map<Items.Item, Items.ItemDto>(products.SingleOrDefault(i => i.Id == l.ProductId));
            }
            return result;
        }

        async public Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersDto input)
        {
            using var session = DocumentStore.LightweightSession();
            var products = await session.Query<Items.Item>().Where(i => i.ProductId != null).ToListAsync();
            var pIds = products.Select(p => p.Id).ToList();
            if (input?.Products?.Any() ?? false)
                pIds = pIds.Where(p => input.Products.Contains(p)).ToList();
            var ordersQueryable = session.Query<ARTMIS.Orders.Order>().Where(o => (o.Lines.Any(_ => pIds.Contains(_.ProductId))) && (((o.ActualDeliveryDate == null) || o.DisplayDate > new DateTime(2021, 12, 1))));
            if (input?.Countries?.Any() ?? false)
                ordersQueryable = ordersQueryable.Where(o => input.Countries.Contains(o.CountryId));

            var totalCount = await ordersQueryable.CountAsync();
            var productIds = products.Select(p => p.ProductId).ToList();
            var items = await ordersQueryable.OrderBy(i => i.CountryId).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
            var result = ObjectMapper.Map<List<ARTMIS.Orders.Order>, List<OrderDto>>(items.ToList());
            foreach (var o in result)
            {
                o.Lines.RemoveAll(l => !pIds.Contains(l.ProductId));
                foreach (var l in o.Lines)
                {                   
                    l.Item = ObjectMapper.Map<Items.Item, Items.ItemDto>(products.SingleOrDefault(i => i.Id == l.ProductId));
                    l.RequestedDeliveryDate = l.RequestedDeliveryDate;
                    l.EstimatedDeliveryDate = l.EstimatedDeliveryDate;
                    l.ActualDeliveryDate = l.ActualDeliveryDate;
                }
            }
            return new PagedResultDto<OrderDto>(totalCount, result);
        }

        public async Task<PagedResultDto<OrderLineDto>> GetShipmentsAsync(GetOrdersDto input)
        {
            using var session = DocumentStore.LightweightSession();
            var products = await session.Query<Items.Item>().ToListAsync();
            var pIds = products.Select(p => p.Id).ToList();
            var linesQueryable = session.Query<ARTMIS.Orders.Order>().SelectMany(o => o.Lines).Where(_ => pIds.Contains(_.ProductId));
            
            var lines = await linesQueryable.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
            var ordersQueryable = session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1)).OrderBy(o => o.CountryId).SelectMany(o => o.Lines).Join(products, o => o.ProductId, p => p.Id, (o, p) => new OrderLineDto
            {
                Item = new Items.ItemDto { Id = p.Id, Name = p.Name, BaseUnitMultiplier = p.BaseUnitMultiplier },
                LineNumber = o.LineNumber,
                OrderedQuantity = o.OrderedQuantity,
                ProductId = o.ProductId,
                ROPrimeLineNumber = o.ROPrimeLineNumber
            });
            var totalCount = await ordersQueryable.CountAsync();
            var result = await ordersQueryable.ToListAsync();
            return new PagedResultDto<OrderLineDto>(totalCount, result);
        }

        async public Task<(List<CountryDto>, List<PeriodDto>)> GetFiltersAsync()
        {
            var countries = (await CountryAppService.GetListAsync(new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto
            {
                MaxResultCount = 100
            })).Items.OrderBy(c => c.Name);
            var periods = (await PeriodAppService.GetListAsync(new GetPeriodListDto
            {
                StartMonth = 12,
                StartYear = 2021,
                EndMonth = 12,
                EndYear = 2022

            })).Items;
            return (countries.ToList(), periods.ToList());
        }
    }
}
