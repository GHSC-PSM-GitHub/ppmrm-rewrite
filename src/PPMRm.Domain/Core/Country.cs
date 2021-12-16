using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Core
{
    public class Country : AuditedAggregateRoot<string>, ISoftDelete
    {
        public Country()
        {
        }

        public Country(string id) : base(id)
        {
        }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
