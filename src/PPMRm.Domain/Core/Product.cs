using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
namespace PPMRm.Core
{
    public class Product : AuditedAggregateRoot<string>, ISoftDelete
    {
        public Product()
        {
        }
        public Product(string id) : base(id)
        {
        }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string BaseUnit { get; set; }
        public decimal BaseUnitMultiplier { get; set; }
        public bool IsDeleted { get; set; }
    }
}
