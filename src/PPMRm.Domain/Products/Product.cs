using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
namespace PPMRm.Products
{
    public class Product : AggregateRoot<string>
    {
        public string Name { get; set; }
        public string TracerCategory { get; set; }
    }
}
