using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace PPMRm.Products
{
    public class ProductDto : EntityDto<string>
    {
        public string Name { get; set; }
        public TracerCategory TracerCategory { get; set; }
    }
}
