using System;
using System.Collections.Generic;
using System.Linq;
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
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public int DefaultProgramId { get; set; }

        public ICollection<CountryProduct> Products { get; private set; }
        public ICollection<CountryProgram> Programs { get; private set; }
        public bool IsDeleted { get; set; }

        private Country() { }

        internal Country(string name, string twoLetterCode, string threeLetterCode, string numericCode) : base(threeLetterCode)
        {
            Name = name;
            TwoLetterCode = twoLetterCode;
            ThreeLetterCode = threeLetterCode;
            NumericCode = numericCode;
            ARTMISName = name;
            UpdateMinMax(PeriodReports.PeriodReportConsts.MOS.Min, PeriodReports.PeriodReportConsts.MOS.Max);
            Products = new List<CountryProduct>();
            Programs = new List<CountryProgram>();
        }

        internal Country(string name, string twoLetterCode, string threeLetterCode, string numericCode, List<int> programIds) : this(name, twoLetterCode, threeLetterCode, numericCode)
        {
            Check.NotNull(programIds, nameof(programIds));
            programIds.ForEach(p => AddProgram(p));
        }

        public void AddProgram(int programId)
        {
            if (!Programs.Any(p => p.ProgramId == programId)) Programs.Add(new CountryProgram(countryId: Id, programId: programId));
        }
        public void UpdateMinMax(int minStock, int maxStock)
        {
            MinStock = minStock;
            MaxStock = maxStock;
        }

        public void AddProduct(string productId)
        {
            Check.NotNull(productId, nameof(productId));
            if (HasProduct(productId)) { return; }
            Products.Add(new CountryProduct(countryId: Id, productId: productId));
        }

        public void RemoveProduct(string productId)
        {
            Check.NotNull(productId, nameof(productId));
            Products.RemoveAll(p => p.ProductId == productId);
        }

        public void RemoveAllExceptGivenIds(List<string> productIds)
        {
            Check.NotNull(productIds, nameof(productIds));
            Products.RemoveAll(x => !productIds.Contains(x.ProductId));
        }

        private bool HasProduct(string productId) => Products.Any(x => x.ProductId == productId);
        
    }
}
