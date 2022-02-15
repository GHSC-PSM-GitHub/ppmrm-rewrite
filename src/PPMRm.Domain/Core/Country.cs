using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PPMRm.Core
{
    //public class Country : AuditedAggregateRoot<string>, ISoftDelete
    //{
    //    public Country()
    //    {
    //    }

    //    public Country(string id) : base(id)
    //    {
    //    }
    //    public string Name { get; set; }
    //    public bool IsDeleted { get; set; }
    //}

    public class Country : AuditedAggregateRoot<string>, ISoftDelete
    {
        public string Name { get; private set; }
        public string TwoLetterCode { get; private set; }
        public string ThreeLetterCode { get; private set; }
        public string NumericCode { get; private set; }
        public string ARTMISName { get; set; }
        public bool IsDeleted { get; set; }

        internal Country(string name, string twoLetterCode, string threeLetterCode, string numericCode) : base(threeLetterCode)
        {
            Name = name;
            TwoLetterCode = twoLetterCode;
            ThreeLetterCode = threeLetterCode;
            NumericCode = numericCode;
            ARTMISName = name;
        }

        
    }
}
