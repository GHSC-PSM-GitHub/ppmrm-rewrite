using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Orders
{
    public class GetOrdersDto : PagedResultRequestDto
    {
        public List<string> Countries { get; set; }
    }
}
