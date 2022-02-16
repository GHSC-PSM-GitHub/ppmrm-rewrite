using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace PPMRm.Products
{
    public class ProductDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        IRepository<Product, string> ProductRepository { get; }
        public ProductDataSeedContributor(IRepository<Product, string> repository)
        {
            ProductRepository = repository;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            if(await ProductRepository.GetCountAsync() == 0)
            {
                var products = ProductConsts.PPMRmProducts.Select(p => new Product(p.Id, p.Name, p.Category)).OrderBy(p => p.Name);
                await ProductRepository.InsertManyAsync(products);
            }
        }
    }
}
