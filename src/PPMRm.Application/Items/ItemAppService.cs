using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;

namespace PPMRm.Items
{
    public class ItemAppService : ApplicationService, IItemAppService
    {
        IDocumentStore DocumentStore { get; }
        public ItemAppService(IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }
        
        async public Task<ItemDto> GetAsync(string id)
        {
            using var session = DocumentStore.LightweightSession();
            var item = await session.LoadAsync<Item>(id);
            return ObjectMapper.Map<Item, ItemDto>(item);

        }

        public async Task<PagedResultDto<ItemDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            using var session = DocumentStore.LightweightSession();
            var totalCount = await session.Query<Item>().Where(i => i.ProductId != null).CountAsync();
            var items = await session.Query<Item>().Where(i => i.ProductId != null).ToListAsync();
            var result = ObjectMapper.Map<List<Item>, List<ItemDto>>(items.ToList());

            var response = result.AsQueryable()
                           .OrderBy(input.Sorting ?? "Name")
                           .Skip(input.SkipCount)
                           .Take(input.MaxResultCount).ToList();
            return new PagedResultDto<ItemDto>(totalCount, response);
        }
    }
}
