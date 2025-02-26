﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Orders
{
    public class GetOrdersDto : PagedAndSortedResultRequestDto
    {
        public List<string> Countries { get; set; }
        public List<string> Products { get; set; }
    }
}
