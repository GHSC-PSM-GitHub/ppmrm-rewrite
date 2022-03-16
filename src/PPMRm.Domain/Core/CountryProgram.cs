using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PPMRm.Core
{
    public class CountryProgram : Entity
    {
        private CountryProgram()
        {

        }
        internal CountryProgram(string countryId, int programId)
        {
            CountryId = Check.NotNullOrEmpty(countryId, nameof(countryId));
            ProgramId = programId;
        }
        public string CountryId { get; private set; }
        public int ProgramId { get; private set; }

        public override object[] GetKeys()
        {
            return new object[] { CountryId, ProgramId };
        }
    }
}
