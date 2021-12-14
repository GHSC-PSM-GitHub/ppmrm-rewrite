using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public class ProductAppService : CrudAppService<
            Product, 
            ProductDto, 
            string, //Primary key 
            PagedAndSortedResultRequestDto,
            ProductDto>, 
        IProductAppService
    {
        public ProductAppService(IRepository<Product, string> repository) : base(repository)
        {
        }
    }
}
