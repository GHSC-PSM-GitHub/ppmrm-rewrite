using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Products
{
    public interface IProductRepository : IReadOnlyRepository<Product, string>
    {
    }
}
