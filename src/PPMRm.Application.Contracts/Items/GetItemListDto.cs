using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Items
{
    public class GetItemListDto : PagedResultRequestDto
    {
        public string TracerCategory { get; set; }
    }
}
