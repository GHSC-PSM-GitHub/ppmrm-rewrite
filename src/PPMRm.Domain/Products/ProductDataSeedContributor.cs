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
            var seedProducts = ProductConsts.PPMRmProducts.Select(p => new Product(p.Id, p.Name, p.Category)).OrderBy(p => p.Name);
            if (await ProductRepository.GetCountAsync() < seedProducts.Count())
            {
                var allProducts = await ProductRepository.GetListAsync();
                var newProductIds = seedProducts.Select(x => x.Id).Except(allProducts.Select(x => x.Id));
                await ProductRepository.InsertManyAsync(seedProducts.Where(x => newProductIds.Contains(x.Id)));
            }
        }
    }
}
