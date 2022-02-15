using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Core
{
    public interface IProductAppService : ICrudAppService< //Defines CRUD methods
            ProductDto, 
            string, //Primary key 
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            ProductDto>
    {
        
    }
}
