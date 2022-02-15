using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.Shipments
{
    public interface IShipmentAppService : IApplicationService
    {
        Task<PagedResultDto<ShipmentDto>> GetListAsync(PagedResultRequestDto input);
    }
}
