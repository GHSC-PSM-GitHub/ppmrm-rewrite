using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Core
{
    public class Program : AuditedAggregateRoot<int>, ISoftDelete
    {
        private Program()
        {

        }
        internal Program(Programs program, string name) : base((int)program)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            Name = name;
        }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
