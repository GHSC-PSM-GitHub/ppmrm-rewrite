using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMRm.Items
{
    public interface IItemRepository
    {
        Task<Item> GetAsync(string id);
        Task<List<Item>> GetListAsync();
    }
}
