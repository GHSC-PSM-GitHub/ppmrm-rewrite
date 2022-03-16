using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PPMRm.Core
{
    public class CountryProduct : Entity
    {
        private CountryProduct()
        {

        }
        internal CountryProduct(string countryId, string productId)
        {
            CountryId = Check.NotNullOrEmpty(countryId, nameof(countryId));
            ProductId = Check.NotNullOrEmpty(productId, nameof(productId));
        }
        public string CountryId { get; private set; }
        public string ProductId { get; private set; }

        public override object[] GetKeys()
        {
            return new object[] { CountryId, ProductId };
        }
    }
}
