using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Core
{
    public class Program : AuditedAggregateRoot<string>, ISoftDelete
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
