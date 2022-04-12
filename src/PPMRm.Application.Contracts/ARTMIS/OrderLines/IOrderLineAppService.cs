using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PPMRm.ARTMIS.OrderLines
{
    public interface IOrderLineAppService : IApplicationService
    {
        Task<OrderLineDto> GetAsync(string id);
        Task<PagedResultDto<OrderLineDto>> GetListAsync(GetOrderLinesDto input);
    }

    public class GetOrderLinesDto : PagedResultRequestDto
    {
        public List<string> Countries { get; set; }
        public List<string> Products { get; set; }
        public DateTime? ShipmentDateAfter { get; set; } = new DateTime(2022, 3, 1);
    }
}
