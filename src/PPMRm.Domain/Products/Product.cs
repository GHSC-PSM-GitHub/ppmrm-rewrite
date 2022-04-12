using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace PPMRm.Products
{
    public class Product : AggregateRoot<string>, ISoftDelete
    {
        public Product(string id, string name, TracerCategory tracerCategory)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNullOrEmpty(name, nameof(id));
            Id = id;
            Name = name;
            TracerCategory = tracerCategory;
        }
        public string Name { get; set; }
        public TracerCategory TracerCategory { get; set; }
        public bool IsDeleted { get; set; }
    }
}
