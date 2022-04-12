using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Items
{
    public interface IItemAppService : IReadOnlyAppService<
            ItemDto,
            string,
            PagedAndSortedResultRequestDto>
    {
    }
}
