using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Core
{
    public class Program : AuditedAggregateRoot<Programs>, ISoftDelete
    {
        internal Program(Programs id, string name) : base(id)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            Name = name;
        }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
