using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
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
            var order = await session.LoadAsync<ARTMIS.Orders.Order>(id);
            return ObjectMapper.Map<ARTMIS.Orders.Order, OrderDto>(order);
        }

        async public Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            using var session = DocumentStore.LightweightSession();
            var totalCount = await session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021,12,1) && o.Lines.Any()).CountAsync();
            var products = await session.Query<Items.Item>().ToListAsync();
            var productIds = products.Select(p => p.ProductId).ToList();
            var items = await session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1) && o.Lines.Any()).OrderBy(i => i.CountryId).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
            var result = ObjectMapper.Map<List<ARTMIS.Orders.Order>, List<OrderDto>>(items.ToList());
            foreach (var o in result)
            {
                foreach (var l in o.Lines)
                {
                    l.Item = ObjectMapper.Map<Items.Item, Items.ItemDto>(products.SingleOrDefault(i => i.Id == l.ProductId));
                }
            }
            return new PagedResultDto<OrderDto>(totalCount, result);
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
