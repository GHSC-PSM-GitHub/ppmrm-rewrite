using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.Items
{
    public class ItemRepository : IItemRepository
    {
        IDocumentSession DocumentSession { get; }
        public ItemRepository(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
        }
        async public Task<Item> GetAsync(string id)
        {
            return await DocumentSession.LoadAsync<Item>(id);
        }

        async public Task<List<Item>> GetListAsync()
        {
            return (await DocumentSession.Query<Item>().ToListAsync()).ToList();
        }
    }
}
