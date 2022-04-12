using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PPMRm.Core
{
    public interface ICountryRepository : IRepository<Country, string>
    {
        Task<ICollection<Country>> GetUserCountriesAsync();
    }
}
