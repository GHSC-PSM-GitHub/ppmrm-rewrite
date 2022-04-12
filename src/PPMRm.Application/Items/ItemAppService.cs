using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
            var items = await session.Query<Item>().Where(i => i.ProductId != null).OrderBy(i => i.Name).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();
            var result = ObjectMapper.Map<List<Item>, List<ItemDto>>(items.ToList());
            return new PagedResultDto<ItemDto>(totalCount, result);
        }
    }
}
